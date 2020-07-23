using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    void Start()
    {
        // event listeners
        GameEvents.current.OnMusicUnMuted += UnMuteMusic;
        GameEvents.current.OnMusicMuted += MuteMusic;

        // set music preferences if not set
        int enabled = PlayerPrefs.GetInt("MusicEnabled", 0);
        if (enabled == 0 || enabled == 1) {
            UnMuteMusic();
        } else {
            MuteMusic();
        }
    }

    void MuteMusic()
    {
        AudioSource[] sources = this.gameObject.GetComponentsInChildren<AudioSource>();
        foreach (AudioSource source in sources) {
            source.mute = true;
        }
        PlayerPrefs.SetInt("MusicEnabled", -1);
    }

    void UnMuteMusic()
    {
        AudioSource[] sources = this.gameObject.GetComponentsInChildren<AudioSource>();
        foreach (AudioSource source in sources) {
            source.mute = false;
        }
        PlayerPrefs.SetInt("MusicEnabled", 1);
    }
}
