using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotController : MonoBehaviour
{
    public GameObject carrot;
    public GameObject carrotDeathParticle;

    // music
    public AudioSource audio;
    public AudioClip CarrotEatenAudio;

    public float healthTimer = 0.0f;
    public float healthMax = 5.0f;

    void Start()
    {
        audio = GameObject.FindWithTag("MainMusic").GetComponent<AudioSource>();
        healthTimer = healthMax;
    }

    void Update()
    {
        if (healthTimer <= 0)
        {
            DestroyCarrot();
        }
        if (getNumWeasels() == 0)
        {
            // wave complete, so restore health 
            healthTimer = healthMax;
        }
    }

    void DestroyCarrot()
    {
        Instantiate(carrotDeathParticle, this.transform.position += Vector3.up * 7.0f, this.transform.rotation);
        carrotDeathParticle.GetComponent<ParticleSystem>().Play();
        PlayCarrotEatenAudio();
        Destroy(carrot);
    }

    void beingEaten(Collider other)
    {
        this.GetComponent<ParticleSystem>().Play();
        other.GetComponentInChildren<Animator>().SetBool("eating", true);
        healthTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weasel"))
        {
            beingEaten(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Weasel"))
        {
            beingEaten(other);
        }
    }

    int getNumWeasels()
    {
        GameObject[] weaselArr = GameObject.FindGameObjectsWithTag("Weasel");
        return weaselArr.Length;
    }

    void PlayCarrotEatenAudio()
    {
        audio.PlayOneShot(CarrotEatenAudio, 2F);
    }
}
