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
    private bool isInvulnerable = false;
    [SerializeField] private float invulnerabilityDuration = 3f;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Transform groundDetect;
    [SerializeField] private bool isGrounded; // Just so we can see in Editor.
    [SerializeField] private float moveForce;
    [SerializeField] private float jumpForce;
    public LayerMask groundLayer;
    private float groundCheckWidth = 2.0f;
    private float groundCheckHeight = 0.25f;
    private Animator an;
    private Rigidbody2D rb;
    private CapsuleCollider2D cc;



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

    public void TakeDamage()
    {
        if (isInvulnerable)
        {
            Debug.Log("Player is invulnerable. No damage taken.");
            return;
        }

        // Reduce health here if you have a health system (e.g., currentHealth--)
        Debug.Log("Player took damage!");

        StartCoroutine(StartInvulnerability());
    }

    private IEnumerator StartInvulnerability()
    {
        isInvulnerable = true;
        Debug.Log("Player is now invulnerable.");

        // Optional: make the sprite blink
        float elapsed = 0f;
        while (elapsed < invulnerabilityDuration)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(0.2f);
            sr.enabled = true;
            yield return new WaitForSeconds(0.2f);
            elapsed += 0.4f;
        }

        isInvulnerable = false;
        Debug.Log("Invulnerability ended.");
    }



}
