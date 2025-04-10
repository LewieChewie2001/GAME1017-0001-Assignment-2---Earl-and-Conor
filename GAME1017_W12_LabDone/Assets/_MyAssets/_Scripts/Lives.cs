using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lives : MonoBehaviour
{
    public int currentLives = 3;
    public UiManager uiManager;

    void Start()
    {
        if (uiManager == null)
            uiManager = FindObjectOfType<UiManager>();

        uiManager.UpdateLivesUI(currentLives);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle")) // or whatever your damaging object is
        {
            currentLives--;
            uiManager.UpdateLivesUI(currentLives);
            Debug.Log("Player hit! Lives left: " + currentLives);
            Debug.Log("Collision detected with: " + collision.gameObject.name);

            if (currentLives <= 0)
            {
                Debug.Log("Player Died");
                SceneManager.LoadScene("Losing"); // <-- Make sure this scene name matches exactly
            }
        }
    }
}


