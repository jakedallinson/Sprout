using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class EndMenuController : MonoBehaviour
{
    public GameObject GameplayUI;
    public GameObject EndMenuUI;

    public GameObject EndText;
    public GameObject HighScore;
    public GameObject HighScoreCounter;

    void Start()
    {
        HighScore.SetActive(false);

        // event listeners
        GameEvents.current.OnNewHighScore += ShowHighScore;
    }

    void Update()
    {
        // check for game over
        if (SpawnController.IsGameOver()) {
            EndMenuUI.SetActive(true);
            EndText.GetComponent<Text>().text = "You survived " + (SpawnController.GetCurrWave() - 1).ToString() + " waves!";
        } else {
            EndMenuUI.SetActive(false);
        }
    }

    private void ShowHighScore()
    {
        HighScore.SetActive(true);
        HighScoreCounter.GetComponent<Text>().text = (SpawnController.CurrWave - 1).ToString();
    }
}
