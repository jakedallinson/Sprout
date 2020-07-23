using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SproutMovement : MonoBehaviour
{
    public Animator animator;
    public CharacterController player;
    public ParticleSystem dust;

    // music
    public AudioSource audio;
    public AudioClip sproutDashAudio;
    public AudioClip sproutRunAudio;
    private bool playingRunAudio;

    // movement variables
    private Vector3 MoveDirection;
    public float moveSpeed = 10f;
    public float turnSpeed = 2f;
    public float GroundDistanceFeeler = 0.1f;

    // dash variables
    public float dashSpeed = 20.0f;
    public float dashSmooth = 0.2f;
    public float maxDashTime = 2.0f;
    public float currDashTime;
    public static bool isDashing;

    // stores values for movement direction
    private float moveX;
    private float moveZ;
    public float lastY;

    // buttons
    public GameObject dashButton;

    float GroundDistance;

    // used to check for movement
    public Vector3 lastPosition;

    void Start()
    {
        audio = GameObject.FindWithTag("SproutMusic").GetComponent<AudioSource>();
        audio.clip = sproutRunAudio;
        playingRunAudio = false;

        player = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        GroundDistance = player.bounds.extents.y;

        Button btn = dashButton.GetComponent<Button>();
        btn.onClick.AddListener(beginDash);

        MoveDirection = Vector3.zero;
        lastPosition = Vector3.zero;
        currDashTime = maxDashTime;
        isDashing = false;
    }

    // our update method
    void Update()
    {
        // get the last Y of the player
        if (IsGrounded())
        {
            lastY = transform.position.y;
        }
        // don't move if game over
        // if (SpawnController.IsGameOver()) {
        //     return;
        // }

        setInputs();
        movePlayer();
        rotatePlayer();
        playAnimations();

        dashPlayer();
    }

    void setInputs()
    {
        // returns between [-1, 1] for movement in respective direction
        moveX = JoystickController.moveVector.x;
        moveZ = JoystickController.moveVector.y;
    }

    // move the player using input
    void movePlayer()
    {
        MoveDirection = new Vector3(moveX * moveSpeed, 0.0f, moveZ * moveSpeed) * Time.deltaTime;
        player.Move(MoveDirection);
        // play running audio if sprout is moving
        if (MoveDirection != Vector3.zero && !playingRunAudio) {
            audio.Play();
            playingRunAudio = true;
        } else if (MoveDirection == Vector3.zero) {
            audio.Pause();
            playingRunAudio = false;
        }
    }

    // rotate player using input
    void rotatePlayer()
    {
        Vector3 lookDir = new Vector3(moveX, 0.0f, moveZ);
        // rotate player as long as looking vector is not the zero vector, or no rotation needed
        if (lookDir != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(lookDir);
        }
    }

    // play animations based on if moving
    // TODO: using walk animation if moving slowly
    void playAnimations()
    {
        // IF: small move, walk
        // ELSE IF: large move, run
        if (lastPosition != gameObject.transform.position)
        {
            animator.SetBool("running", true);
        } else {
            animator.SetBool("running", false);
        }
        lastPosition = gameObject.transform.position;
    }

    void beginDash()
    {
        // if (!isDashing)
        // {
        //     // just started dashing so play sound effect
        //     playSproutDashAudio();
        // }
        if (DashCooldown.dashAllowed && MoveDirection != Vector3.zero)
        {
            DashCooldown.dashUsed = true;
            currDashTime = 0.0f;
            isDashing = true;
            CreateDust();
            playSproutDashAudio();
        }
    }

    void dashPlayer()
    {
        //if (Input.GetKeyDown(KeyCode.Space) && (currDashTime >= maxDashTime))

        // increment currDash until it matches maxDash
        Vector3 moveDir;
        if (currDashTime < maxDashTime) {
            moveDir = new Vector3(moveX * dashSpeed, 0.0f, moveZ * dashSpeed) * Time.deltaTime;
            currDashTime += dashSmooth;
        } else {
            // done dashing, set to zero
            moveDir = Vector3.zero;
            isDashing = false;
        }
        player.Move(moveDir);
    }

    /// Determines whether the sprout is on the ground or not by his y
    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, GroundDistance + GroundDistanceFeeler);
    }

    void CreateDust()
    {
        dust.Play();
    }

    void playSproutDashAudio()
    {
        audio.PlayOneShot(sproutDashAudio, 1.0F);
    }
}

