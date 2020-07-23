using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameplayVisualsController : MonoBehaviour
{
    public GameObject GamePlayUI;
    public GameObject CarrotCounter;
    public GameObject WeaselCounter;
    public GameObject WaveText;
    public GameObject WaveUpdate;

    private float WaveUpdateDelay = 3.0f; // how long to show the update
    private float WaveUpdateTimer;

    void Start()
    {
        WaveUpdateTimer = WaveUpdateDelay;
        ShowGamePlayUI();
        // event listeners
        GameEvents.current.OnPowerUpStarted += HideGamePlayUI;
        GameEvents.current.OnPowerUpFinished += ShowGamePlayUI;
        GameEvents.current.OnAdFinished += ShowGamePlayUI;
    }

    void Update()
    {
        if (SpawnController.IsGameOver()) {
            HideGamePlayUI();
        }
        ChangeCounters();
        ChangeWaveText();
        ChangeWaveUpdate();
    }

    void ChangeCounters()
    {
        CarrotCounter.GetComponent<Text>().text = getNumCarrots().ToString();
        WeaselCounter.GetComponent<Text>().text = getNumWeasels().ToString();
    }

    void ChangeWaveText()
    {
        if (SpawnController.GetCurrWave() == 0) {
            WaveText.GetComponent<Text>().text = "Waves starting";
        } else {
            if (SpawnController.IsWaveWaiting()) {
                double time = Math.Ceiling(SpawnController.WaveTimer);
                WaveText.GetComponent<Text>().text = "Next wave in " + time.ToString();
            } else {
                WaveText.GetComponent<Text>().text = "Wave " + SpawnController.GetCurrWave().ToString();
            }
        }
    }

    void ChangeWaveUpdate()
    {
        if (SpawnController.IsWaveComplete()) {
            WaveUpdate.SetActive(true);
        }
        // show the update for timer seconds
        if (WaveUpdate.activeSelf) {
            WaveUpdateTimer -= Time.deltaTime;
        }
        if (WaveUpdateTimer <= 0.0) {
            WaveUpdateTimer = WaveUpdateDelay;
            WaveUpdate.SetActive(false);
        }
    }

    private void HideGamePlayUI()
    {
        GamePlayUI.SetActive(false);
    }

    private void ShowGamePlayUI()
    {
        GamePlayUI.SetActive(true);
    }

    int getNumCarrots()
    {
        GameObject[] carrotArr = GameObject.FindGameObjectsWithTag("Carrot");
        return carrotArr.Length;
    }

    int getNumWeasels()
    {
        GameObject[] weaselArr = GameObject.FindGameObjectsWithTag("Weasel");
        return weaselArr.Length;
    }
}
