using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    //
    // SPROUT'S EVENT SYSTEM
    // class holds events relevant to game logic
    //

    public static GameEvents current;
    
    private void Awake()
    {
        current = this;
    }

    //
    // SPAWN
    // 
    public event Action OnNewHighScore;
    public void NewHighScore()
    {
        if (OnNewHighScore != null)
        {
            OnNewHighScore();
        }
    }

    //
    // POWER UPS
    //
    public event Action OnPowerUpStarted;
    public void PowerUpStarted()
    {
        if (OnPowerUpStarted != null)
        {
            OnPowerUpStarted();
        }
    }

    public event Action OnPowerUpFinished;
    public void PowerUpFinished()
    {
        if (OnPowerUpFinished != null)
        {
            OnPowerUpFinished();
        }
    }

    public event Action OnTouchOutsideGarden;
    public void TouchOutsideGarden()
    {
        if (OnTouchOutsideGarden != null)
        {
            OnTouchOutsideGarden();
        }
    }

    //
    // ADS
    //
    public event Action OnAdStarted;
    public void AdStarted()
    {
        if (OnAdStarted != null)
        {
            OnAdStarted();
        }
    }

    public event Action OnAdFinished;
    public void AdFinished()
    {
        if (OnAdFinished != null)
        {
            OnAdFinished();
        }
    }

    public event Action OnAdErrored;
    public void AdErrored()
    {
        if (OnAdErrored != null)
        {
            OnAdErrored();
        }
    }

    //
    // MUSIC
    //
    public event Action OnMusicMuted;
    public void MusicMuted()
    {
        if (OnMusicMuted != null)
        {
            OnMusicMuted();
        }
    }

    public event Action OnMusicUnMuted;
    public void MusicUnMuted()
    {
        if (OnMusicUnMuted != null)
        {
            OnMusicUnMuted();
        }
    }
}
