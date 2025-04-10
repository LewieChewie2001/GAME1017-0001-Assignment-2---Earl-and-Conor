using UnityEngine;
using UnityEngine.SceneManagement;

public class Lives : MonoBehaviour
{
    public int currentLives = 3; // Number of lives the player starts with
    public UiManager uiManager; // Reference to the UI manager to update lives display

    // Invulnerability variables
    private bool isInvulnerable = false; // Tracks if the player is currently invulnerable
    private float invulnerabilityDuration = 5f; // Duration of invulnerability after hitting an obstacle
    private float invulnerabilityTimer = 0f; // Timer to track how long invulnerability lasts

    void Start()
    {
        // If UI manager is not assigned, try to find it
        if (uiManager == null)
            uiManager = FindObjectOfType<UiManager>();

        // Update UI with the initial number of lives
        uiManager.UpdateLivesUI(currentLives);
    }

    void Update()
    {
        // Countdown the invulnerability timer if the player is invulnerable
        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;

            // When the timer runs out, stop invulnerability
            if (invulnerabilityTimer <= 0f)
            {
                isInvulnerable = false;
                Debug.Log("Invulnerability ended.");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Don't process collision if the player is invulnerable
        if (isInvulnerable)
        {
            Debug.Log("Player is invulnerable, no damage taken.");
            return;
        }

        // Check for collision with obstacles
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Start invulnerability after hitting an obstacle
            isInvulnerable = true;
            invulnerabilityTimer = invulnerabilityDuration; // Reset invulnerability timer
            Debug.Log("Player is now invulnerable!");

            // Decrease lives only if the player is not invulnerable
            currentLives--;
            uiManager.UpdateLivesUI(currentLives); // Update the UI to show the new number of lives
            Debug.Log("Player hit! Lives left: " + currentLives);

            // If the player runs out of lives, transition to losing scene
            if (currentLives <= 0)
            {
                Debug.Log("Player Died");
                SceneManager.LoadScene("Losing"); // Load the losing scene
            }
        }
    }
}



