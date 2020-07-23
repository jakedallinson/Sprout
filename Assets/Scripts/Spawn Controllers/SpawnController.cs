using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    // objects
    public GameObject Weasel;
    public GameObject Carrot;
    public Camera OverheadCamera;

    // music
    private AudioSource audio;
    public AudioClip WaveCompleteAudio;
    public AudioClip WeaselSpawnAudio;
    public AudioClip PowerUpAudio;

    public enum WaveState {Start, Complete, Waiting, Spawning, Finished, GameOver, Reward};
    public static WaveState State;
    public static int CurrWave;

    // wave variables
    private int WaveWeaselIncrement = 2;        // weasels spawned is this * wave number
    private int WaveCarrotIncrement = 2;        // carrots added after each wave
    private float WaveDelay         = 5.0f;
    public static float WaveTimer;

    // spawn variables
    private int CarrotsToStart      = 20;       // num of carrots starting on map
    private int WeaselsSpawned      = 0;        // weasels spawned for an individual wave
    private float SpawnDelay        = 0.5f;     // time between each weasel spawn
    private float SpawnTimer;

    // power up variables
    public GameObject CarrotPowerUp;
    // public GameObject BombPowerUp;
    private int PowerUpFrequency = 4;           // power ups every n waves

    // current map has radius of 27.5f
    public float MapRadius = 25.0f;

    void Start()
    {
        State = WaveState.Start;
        CurrWave = 1;
        WaveTimer = WaveDelay;
        SpawnTimer = SpawnDelay;
        audio = GameObject.FindWithTag("SpawnMusic").GetComponent<AudioSource>();

        // event listeners
        GameEvents.current.OnPowerUpFinished += RestartWave;
        GameEvents.current.OnAdFinished += SpawnRewardForAd;
    }

    void Update()
    {
        // look at current wave state
        if (State == WaveState.Start) {
            // START
            // start of game by spawning the carrots inside map radius
            SpawnNCarrots(CarrotsToStart);
            State = WaveState.Waiting;
        } else if (State == WaveState.Complete) {
            // COMPLETE
            // wave was just complete
            SpawnNCarrots(WaveCarrotIncrement);
            if (CurrWave % PowerUpFrequency == 0) {
                SpawnPowerUp(true);
            }
            PlayWaveCompleteAudio();
            CurrWave++;
            State = WaveState.Waiting;
        } else if (State == WaveState.Waiting) {
            // WAITING
            // wave has been completed, waiting to start spawning next wave
            WaveTimer -= Time.deltaTime;
            if (WaveTimer <= 0.0f) {
                WaveTimer = WaveDelay;
                State = WaveState.Spawning;
            }
        } else if (State == WaveState.Spawning) {
            // SPAWNING
            // spawning next wave
            SpawnTimer -= Time.deltaTime;
            if (SpawnTimer <= 0.0f) {
                spawnWeasel();
                PlayWeaselSpawnAudio();
                SpawnTimer = SpawnDelay;
                WeaselsSpawned++;
            }
            // spawn curr wave x weasel increment
            if (WeaselsSpawned >= CurrWave * WaveWeaselIncrement) {
                WeaselsSpawned = 0;
                State = WaveState.Finished;
            }
        } else if (State == WaveState.Finished) {
            // FINISHED
            // finished spawning, wait for wave to be complete
            if (GetNumWeasels() == 0) {
                State = WaveState.Complete;
            } else if (GetNumCarrots() == 0) {
                State = WaveState.GameOver;
            }
        } else if (State == WaveState.GameOver) {
            // check for new high score
            CheckForHighScore();
        }
        // GAMEOVER or REWARD     
    }

    private void CheckForHighScore()
    {
        int CurrHighScore = PlayerPrefs.GetInt("HighScore", 0);
        if (CurrWave - 1 > CurrHighScore && CurrHighScore > 0) {
            PlayerPrefs.SetInt("HighScore", CurrWave - 1);
            GameEvents.current.NewHighScore();
        }
    }

    private void RestartWave()
    {
        State = WaveState.Waiting;
    }

    public static bool IsGameOver()
    {
        return State == WaveState.GameOver;
    }

    public static bool IsWaveStart()
    {
        return State == WaveState.Start;
    }

    public static bool IsWaveComplete()
    {
        return State == WaveState.Complete;
    }

    public static bool IsWaveWaiting()
    {
        return State == WaveState.Waiting;
    }

    public static int GetCurrWave()
    {
        return CurrWave;
    }

    public static float GetWaveTimer()
    {
        return WaveTimer;
    }

    private void SpawnRewardForAd()
    {
        State = WaveState.Reward;
        SpawnPowerUp(false);
        DestroyAllWeasels();
    }


    // spawn n number of carrots inside map radius
    void SpawnNCarrots(int NumCarrots)
    {
        for (int i = 0; i < NumCarrots; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-MapRadius, MapRadius), -7.0f, Random.Range(-MapRadius, MapRadius));
            Instantiate(Carrot, pos, Carrot.transform.rotation);
        }
    }

    public void SpawnCarrotAtPoint(Vector3 point)
    {
        Vector3 pos = new Vector3(point.x, -7.0f, point.z);
        Instantiate(Carrot, pos, Carrot.transform.rotation);
    }

    void SpawnPowerUp(bool random)
    {
        PlayPowerUpAudio();
        if (random) {
            Vector3 pos = new Vector3(Random.Range(-MapRadius, MapRadius), 2.5f, Random.Range(-MapRadius, MapRadius));
            Instantiate(CarrotPowerUp, pos, CarrotPowerUp.transform.rotation);
        } else {
            Vector3 pos = new Vector3(0.0f, 2.5f, 0.0f);
            Instantiate(CarrotPowerUp, pos, CarrotPowerUp.transform.rotation);
        }
    }

    void spawnWeasel()
    {
        Vector3 weaselPos;
        Quaternion weaselRot = new Quaternion(0,0,0,1);
        switch (GetRandom())
        {
            case 1:
                weaselPos = new Vector3(Random.Range(-MapRadius, MapRadius), 0.0f, MapRadius); 
                weaselRot.SetLookRotation(Vector3.Normalize(-weaselPos), Vector3.up);
                Instantiate(Weasel, weaselPos, weaselRot);
                break;
            case 2:
                weaselPos = new Vector3(MapRadius, 0.0f, Random.Range(-MapRadius, MapRadius)); 
                weaselRot.SetLookRotation(Vector3.Normalize(-weaselPos), Vector3.up);
                Instantiate(Weasel, weaselPos, weaselRot);
                break;
            case 3:
                weaselPos = new Vector3(-MapRadius, 0.0f, Random.Range(-MapRadius, MapRadius)); 
                weaselRot.SetLookRotation(Vector3.Normalize(-weaselPos), Vector3.up);
                Instantiate(Weasel, weaselPos, weaselRot);
                break;
            default:
                weaselPos = new Vector3(Random.Range(-MapRadius, MapRadius), 0.0f, -MapRadius); 
                weaselRot.SetLookRotation(Vector3.Normalize(-weaselPos), Vector3.up);
                Instantiate(Weasel, weaselPos, weaselRot);
                break;
        }
    }

    int GetRandom()
    {
        return Random.Range(1, 5);
    }

    int GetNumCarrots()
    {
        GameObject[] carrotArr = GameObject.FindGameObjectsWithTag("Carrot");
        return carrotArr.Length;
    }

    int GetNumWeasels()
    {
        GameObject[] weaselArr = GameObject.FindGameObjectsWithTag("Weasel");
        return weaselArr.Length;
    }

    void DestroyAllWeasels()
    {
        GameObject[] WeaselArr = GameObject.FindGameObjectsWithTag("Weasel");
        foreach (GameObject weasel in WeaselArr)
        {
            Destroy(weasel);
        }
    }

    void PlayWeaselSpawnAudio()
    {
        audio.PlayOneShot(WeaselSpawnAudio, 0.7F);
    }
    void PlayWaveCompleteAudio()
    {
        audio.PlayOneShot(WaveCompleteAudio, 0.7F);
    }
    void PlayPowerUpAudio()
    {
        audio.PlayOneShot(PowerUpAudio, 0.7F);
    }
}
