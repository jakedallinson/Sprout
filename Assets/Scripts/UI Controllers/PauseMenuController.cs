using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject PauseMenuUI;
    public GameObject GamePlayUI;
    public GameObject PauseButton;
    public GameObject ResumeButton;

    void Start()
    {
        PauseMenuUI.SetActive(false);
        PauseButton.GetComponent<Button>().onClick.AddListener(Pause);
        ResumeButton.GetComponent<Button>().onClick.AddListener(Resume);
    }

    void Pause()
    {
        GamePlayUI.SetActive(false);
        PauseMenuUI.SetActive(true);
        PauseButton.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    void Resume()
    {
        GamePlayUI.SetActive(true);
        PauseMenuUI.SetActive(false);
        PauseButton.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
}
