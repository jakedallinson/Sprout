using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject MainMenuUI;
    public GameObject AboutMenuUI;
    public GameObject InstructionsMenuUI;
    
    public GameObject InstructionsButton;
    public GameObject AboutButton;
    public GameObject BackButton;
    public GameObject HighScore;
    public GameObject HighScoreCounter;

    // void Awake()
    // {
    //     PlayerPrefs.DeleteAll();
    // }

    void Start()
    {
        // if paused from game
        Time.timeScale = 1f;
        
        AboutButton.GetComponent<Button>().onClick.AddListener(About);
        InstructionsButton.GetComponent<Button>().onClick.AddListener(Instructions);
        BackButton.GetComponent<Button>().onClick.AddListener(Back);
        
        // PlayerPrefs.SetInt("HighScore", 0);
        Back();
        SetHighScore();
    }

    void SetHighScore()
    {
        int score = PlayerPrefs.GetInt("HighScore", 0);
        if (score > 0) {
            HighScore.SetActive(true);
            HighScoreCounter.GetComponent<Text>().text = score.ToString();
        } else {
            HighScore.SetActive(false);
        }
    }

    void Instructions()
    {
        MainMenuUI.SetActive(false);
        InstructionsMenuUI.SetActive(true);
        AboutMenuUI.SetActive(false);
        BackButton.SetActive(true);
    }

    void About()
    {
        MainMenuUI.SetActive(false);
        InstructionsMenuUI.SetActive(false);
        AboutMenuUI.SetActive(true);
        BackButton.SetActive(true);
    }

    void Back()
    {
        MainMenuUI.SetActive(true);
        InstructionsMenuUI.SetActive(false);
        AboutMenuUI.SetActive(false);
        BackButton.SetActive(false);
    }
}
