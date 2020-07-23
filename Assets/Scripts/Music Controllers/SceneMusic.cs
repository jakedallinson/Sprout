using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    // scene tracks
    public AudioClip StartScreenAudio;
    public AudioClip MainGameAudio;

    private Scene scene;
    private AudioSource audio;

    void Start()
    {
        audio = this.GetComponent<AudioSource>();
        PlayMainMusic();
    }

    void PlayMainMusic()
    {
        // set the clip for the scene
        scene = SceneManager.GetActiveScene();
        if (scene.buildIndex == 0) {
            audio.clip = StartScreenAudio;
        } else {
            audio.clip = MainGameAudio;
        }
        audio.Play();
    }
}
