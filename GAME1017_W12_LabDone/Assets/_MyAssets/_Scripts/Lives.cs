using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lives : MonoBehaviour
{
    public int currentLives = 3;
    public UiManager uiManager;

    // Invulnerability Variables
    private bool isInvulnerable = false;
    private float invulnerabilityDuration = 5f; // Duration of invulnerability in seconds
    private float invulnerabilityTimer = 0f; // Timer to track the invulnerability duration

    void Start()
    {
        if (uiManager == null)
            uiManager = FindObjectOfType<UiManager>();

        uiManager.UpdateLivesUI(currentLives);
    }

    void Update()
    {
        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;

            if (invulnerabilityTimer <= 0f)
            {
                isInvulnerable = false;
                Debug.Log("Invulnerability ended.");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player is invulnerable, no damage will be taken.
        if (isInvulnerable)
        {
            Debug.Log("Player is invulnerable, no damage taken.");
            return;
        }

        if (collision.gameObject.CompareTag("Obstacle")) // or whatever your damaging object is
        {
            // Start invulnerability after hitting an obstacle
            isInvulnerable = true;
            invulnerabilityTimer = invulnerabilityDuration; // Set the invulnerability timer to the specified duration
            Debug.Log("Player is now invulnerable!");

            // Decrease lives only if the player is not invulnerable
            currentLives--;
            uiManager.UpdateLivesUI(currentLives);
            Debug.Log("Player hit! Lives left: " + currentLives);

            if (currentLives <= 0)
            {
                Debug.Log("Player Died");
                SceneManager.LoadScene("Losing"); // <-- Make sure this scene name matches exactly
            }
        }
    }
}



