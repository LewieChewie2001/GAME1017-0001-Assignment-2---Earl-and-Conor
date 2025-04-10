using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    Idling,
    Running,
    Rolling,
    Jumping
}

public class PlayerScript : MonoBehaviour, IReceiver
{
    [SerializeField] private Transform groundDetect;
    [SerializeField] private bool isGrounded; // Just so we can see in Editor.
    [SerializeField] private float moveForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private Collider2D jumpOverTriggerZone;
    public LayerMask groundLayer;
    private float groundCheckWidth = 2.0f;
    private float groundCheckHeight = 0.25f;
    private Animator an;
    private Rigidbody2D rb;
    private CapsuleCollider2D cc;

    private int rollsUnderObstacles = 0;
    private int jumpOverObstacles = 0;

    private CharacterState currentState;
    private bool jumpStarted;

    // TODO: Add to Week 12 lab.
    public List<IObserver> Observers { get; private set; } = new List<IObserver>();

    void Start()
    {
        an = GetComponentInChildren<Animator>();
        isGrounded = false; // Always start in air.
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        // TODO: Add to Week 12 lab.
        AchievementObserver achievementObserver = new AchievementObserver(); // Assuming you have a default constructor or another way to instantiate it
        this.AddObserver(achievementObserver);

        StartJump();
    }

    void Update()
    {
        if (!jumpStarted)
        {
            GroundedCheck();
        }
        switch (currentState)
        {
            case CharacterState.Idling:
                HandleIdlingState();
                break;
            case CharacterState.Running:
                HandleRunningState();
                break;
            case CharacterState.Rolling:
                HandleRollingState();
                break;
            case CharacterState.Jumping:
                HandleJumpingState();
                break;
        }
    }

    private void HandleIdlingState()
    {
        // Idling state logic here.
        transform.Translate(new Vector3(-4f * Time.deltaTime, 0f, 0f));
        // Transition to other states.
        if (isGrounded && (Input.GetAxis("Horizontal") != 0))
        {
            an.SetBool("isMoving", true);
            currentState = CharacterState.Running;
        }
        else if (isGrounded && Input.GetButtonDown("Jump"))
        {
            // TODO: Add to Week 12 lab.
            this.NotifyObservers(Event.PlayerJumped);

            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
            Game.Instance.SOMA.PlaySound("Jump");
            StartJump();
        }
        else if (isGrounded && Input.GetKeyDown(KeyCode.S))
        {
            // TODO: Add to Week 12 lab.
            this.NotifyObservers(Event.FiveRolls);

            cc.offset = new Vector2(0.33f, -1f);
            cc.size = new Vector2(2f, 2f);
            Game.Instance.SOMA.PlayLoopedSound("Roll");
            an.SetBool("isRolling", true);
            currentState = CharacterState.Rolling;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player has hit an obstacle
        if (other.CompareTag("Obstacle"))
        {
            // Check if the player is jumping and grounded
            if (currentState == CharacterState.Jumping)
            {
                jumpOverObstacles++;
                Debug.Log("Jump Over Obstacle");

                // Check if the player has jumped over 10 obstacles
                if (jumpOverObstacles >= 2)
                {
                    this.NotifyObservers(Event.JumpedOver10Obstacles);
                }
            }

            // Check if the player is rolling and grounded
            if (currentState == CharacterState.Rolling && isGrounded)
            {
                rollsUnderObstacles++;
                Debug.Log("Rolled Under Obstacle");

                // Check if the player has rolled under 10 obstacles
                if (rollsUnderObstacles >= 2)
                {
                    this.NotifyObservers(Event.RollUnder10Obstacles);
                }
            }
        }
    }

    private void HandleRunningState()
    {
        // Running state logic here.
        MoveCharacter();
        // Transition to other states.
        if (isGrounded && (Input.GetAxis("Horizontal") == 0))
        {
            an.SetBool("isMoving", false);
            currentState = CharacterState.Idling;
        }
        else if (isGrounded && Input.GetButtonDown("Jump"))
        {
            // TODO: Add to Week 12 lab.
            this.NotifyObservers(Event.PlayerJumped);

            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
            Game.Instance.SOMA.PlaySound("Jump");
            an.SetBool("isMoving", false);
            StartJump();
        }
        else if (isGrounded && Input.GetKeyDown(KeyCode.S))
        {
            // TODO: Add to Week 12 lab.
            this.NotifyObservers(Event.FiveRolls);

            an.SetBool("isMoving", false);
            an.SetBool("isRolling", true);
            cc.offset = new Vector2(0.33f, -1f);
            cc.size = new Vector2(2f, 2f);
            Game.Instance.SOMA.PlayLoopedSound("Roll");
            currentState = CharacterState.Rolling;
        }
    }

    private void HandleRollingState()
    {
        // Rolling state logic here.
        MoveCharacter();
        // Transition to other states.
        if (Input.GetKeyUp(KeyCode.S))
        {
            an.SetBool("isRolling", false);
            cc.offset = new Vector2(0.33f, -0.25f);
            cc.size = new Vector2(2f, 3.5f);
            Game.Instance.SOMA.StopLoopedSound();
            currentState = CharacterState.Idling;
        }
    }

    private void HandleJumpingState()
    {
        // Jumping state logic here.
        MoveCharacter();
        // Transition to other states.
        if (isGrounded)
        {
            an.SetBool("isJumping", false);
            currentState = CharacterState.Idling;
        }
    }

    private void MoveCharacter()
    {
        // Horizontal movement.
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveForce * Time.fixedDeltaTime, rb.velocity.y);
    }

    private void GroundedCheck()
    {
        isGrounded = Physics2D.OverlapBox(groundDetect.position,
            new Vector2(groundCheckWidth, groundCheckHeight), 0f, groundLayer);
        an.SetBool("isJumping", !isGrounded);
    }

    private void StartJump()
    {
        jumpStarted = true;
        isGrounded = false;
        currentState = CharacterState.Jumping;
        an.SetBool("isJumping", true);
        Invoke("ResetJumpStarted", 0.5f);
    }

    private void ResetJumpStarted()
    {
        Debug.Log("Resetting jumpStarted.");
        jumpStarted = false;
    }
}
