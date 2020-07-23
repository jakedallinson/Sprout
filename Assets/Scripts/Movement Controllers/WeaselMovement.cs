using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class WeaselMovement : MonoBehaviour
{
    public GameObject enemy;
    public NavMeshAgent agent;
    public Vector3 randPos;
    public Animator animator;

    // particles
    public ParticleSystem SpawnParticle;
    public GameObject StompParticle;

    public GameObject prevTarget;

    // music
    public AudioClip StompWeaselAudio;
    public AudioSource audio;


    // currently map has radius of 27.5f
    private float mapRadius = 25.0f;
    private float FIXED_MIN = 7.1f;

    void Start()
    {
        audio = GameObject.FindWithTag("SproutMusic").GetComponent<AudioSource>();
        // movement
        randPos = Vector3.zero;
        agent = GetComponent<NavMeshAgent>();
        animator = this.GetComponentInChildren<Animator>();
        animator.SetBool("eating", false);
        SpawnParticle = this.GetComponent<ParticleSystem>();
        SpawnParticle.Play();
    }

    void Update()
    {
        // build arr of carrots
        GameObject minCarrot = getMinCarrot();

        if (minCarrot == null)
        {
            // no carrots so move random
            animator.SetBool("eating", false);
            moveRandom();
        }
        else if (animator.GetBool("eating"))
        {
            // weasel is still eating so dont move anymore
            agent.SetDestination(this.transform.position);
        }
        else
        {
            // not eating so move to min carrot
            agent.SetDestination(minCarrot.transform.position);
        }

        // check if not eating anymore
        if (minCarrot != prevTarget)
        {
            // new target so moving again
            animator.SetBool("eating", false);
        }
        prevTarget = minCarrot;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && SproutMovement.isDashing)
        {
            PlayStompWeaselAudio();
            GameObject part = Instantiate(StompParticle, this.transform.position += Vector3.up * 0.5f, this.transform.rotation);
            part.GetComponent<ParticleSystem>().Play();
            Destroy(enemy);
        }
    }

    private GameObject getMinCarrot()
    {
        GameObject[] carrotArr = GameObject.FindGameObjectsWithTag("Carrot");
        GameObject minCarrot = null;
        float minDist = 9999;
        foreach (GameObject carrot in carrotArr)
        {
            if (Vector3.Distance(this.transform.position, carrot.transform.position) < minDist)
            {
                minDist = Vector3.Distance(this.transform.position, carrot.transform.position);
                minCarrot = carrot;
            }
        }
        return minCarrot;
    }

    void moveRandom()
    {
        float magnitude = Vector3.Distance(this.transform.position, randPos);
        if (randPos == Vector3.zero || magnitude < FIXED_MIN)
        {
            // need new random position
            randPos = new Vector3(Random.Range(-mapRadius, mapRadius), 0.0f, Random.Range(-mapRadius, mapRadius));
            agent.SetDestination(randPos);
        }
    }

    void PlayStompWeaselAudio()
    {
        audio.PlayOneShot(StompWeaselAudio, 1f);
    }
}
