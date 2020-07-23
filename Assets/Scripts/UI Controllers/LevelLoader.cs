using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    private AsyncOperation operation;
    private Canvas canvas;
    // private CanvasGroup cg;
    private VideoPlayer video;

    // void Awake()
    // {
    //     DontDestroyOnLoad(this.gameObject);
    // }

    void Start()
    {
        canvas = this.GetComponentInChildren<Canvas>();
        // cg = this.GetComponentInChildren<CanvasGroup>();
        video = this.GetComponentInChildren<VideoPlayer>();
        canvas.enabled = false;
        video.Play();
    }

    public void LoadScene(int SceneIndex)
    {
        // fade in
        canvas.enabled = true;
        //video.Play();
        // StartCoroutine(FadeAlphaIn(SceneIndex, 0.0f, 1.0f));
        StartCoroutine(BeginLoad(SceneIndex));
    }

    // private IEnumerator FadeAlphaIn(int SceneIndex, float start, float end)
    // {
    //     for (float alpha = start; alpha < end; alpha += 0.2f)
    //     {
    //         cg.alpha = alpha;
    //         yield return null;
    //     }
    //     // done fading
    //     cg.alpha = end;
    //     StartCoroutine(BeginLoad(SceneIndex));
    // }

    private IEnumerator BeginLoad(int SceneIndex)
    {
        operation = SceneManager.LoadSceneAsync(SceneIndex);
        while (!operation.isDone) {
            yield return null;
        }
        // done loading
        operation = null;
        canvas.enabled = false;
    }
}
