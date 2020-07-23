using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsController : MonoBehaviour
{
    public GameObject Carrot;
    private Camera OverheadCamera;

    public static int CarrotsPlanted = 0;
    public static int CarrotsToPlant = 5;

    // variables for bouncing power up
    private float MaxY = 5.0f;
    private float MinY = 2.5f;
    private float CountSmooth = 0.1f;
    private float YCounter;
    private bool YCounterDirection;

    public enum PowerUpType {None, Plant, Bomb};
    public PowerUpType Type;

    void Start()
    {
        YCounter = MinY;
        YCounterDirection = true;
        Type = PowerUpType.None;
        OverheadCamera = GameObject.FindGameObjectWithTag("OverheadCamera").GetComponent<Camera>();
    }

    void Update()
    {
        // bounce the power up after it spawns
        BouncePowerUp();
        // check for power up type
        if (Type == PowerUpType.Plant) {
            foreach (Touch touch in Input.touches) {
                // only recognize the first touch
                if (touch.phase == TouchPhase.Began) {
                    // ray from the current touch coordinates
                    Ray ray = OverheadCamera.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100)) {
                        print(hit.collider.tag);
                        if (hit.collider.tag == "Garden" || hit.collider.tag == "Carrot") {
                            SpawnCarrotAtPoint(hit.point);
                            CarrotsPlanted++;
                            // check if last carrot was planted
                            if (CarrotsPlanted >= CarrotsToPlant) {
                                CarrotsPlanted = 0;
                                PowerUpDeactivate();
                            }
                        } else {
                            GameEvents.current.TouchOutsideGarden();
                        }
                    }
                }
            }
        } else if (Type == PowerUpType.Plant) {
            // TODO: implement bomb power up
            PowerUpDeactivate();
        }
    }

    private void BouncePowerUp()
    {
        // bounce power up in y
        if (YCounterDirection) {
            // count up
            YCounter += CountSmooth;
            if (YCounter >= MaxY) {
                YCounterDirection = !YCounterDirection;
                YCounter = MaxY;
            }
        } else {
            // count down
            YCounter -= CountSmooth;
            if (YCounter <= MinY) {
                YCounterDirection = !YCounterDirection;
                YCounter = MinY;
            }
        }
        Vector3 pos = this.transform.position;
        pos.y = YCounter;
        this.transform.position = pos;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player") {
            PowerUpActivate(this.tag);
            // this.gameObject.SetActive(false);
        }
    }

    private void PowerUpActivate(string tag)
    {
        GameEvents.current.PowerUpStarted();
        Time.timeScale = 0f;
        // type of power up interacted with is based on tag
        if (tag == "CarrotPowerUp") {
            Type = PowerUpType.Plant;
        } else if (tag == "BombPowerUp") {
            Type = PowerUpType.Bomb;
        }
    }

    private void PowerUpDeactivate()
    {
        GameEvents.current.PowerUpFinished();
        Time.timeScale = 1.0f;
        Destroy(this.gameObject);
    }

    private void SpawnCarrotAtPoint(Vector3 point)
    {
        Vector3 pos = new Vector3(point.x, -7.0f, point.z);
        Instantiate(Carrot, pos, Carrot.transform.rotation);
    }

    public static int CarrotsLeft()
    {
        return (CarrotsToPlant - CarrotsPlanted);
    }
}
