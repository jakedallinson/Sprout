using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SoundButtonController : MonoBehaviour
{
    public GameObject SoundButtonImage;
    public Sprite Muted;
    public Sprite UnMuted;

    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(ToggleAudio);
    }

    void Update()
    {
        ChangeGraphic();
    }

    void ToggleAudio()
    {
        int enabled = PlayerPrefs.GetInt("MusicEnabled", 0);
        if (enabled == 1 || enabled == 0) {
            GameEvents.current.MusicMuted();
        } else {
            GameEvents.current.MusicUnMuted();
        }
    }

    void ChangeGraphic()
    {
        int enabled = PlayerPrefs.GetInt("MusicEnabled", 0);
        if (enabled == 1 || enabled == 0) {
            SoundButtonImage.GetComponent<Image>().sprite = UnMuted;
        } else {
            SoundButtonImage.GetComponent<Image>().sprite = Muted;
        }
    }
}
