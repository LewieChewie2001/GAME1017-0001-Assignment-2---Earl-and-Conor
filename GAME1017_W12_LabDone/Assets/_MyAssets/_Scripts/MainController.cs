using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    // Singleton instance for MainController
    public static MainController Instance { get; private set; }

    // References to other managers
    public SoundManager soundManager;
    public UiManager uiManager;

    private void Awake()
    {
        // Implement Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: If you want this to persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize the SoundManager instance
        if (soundManager == null)
        {
            if (SoundManager.Instance == null)  // Ensure that the SoundManager is initialized
            {
                GameObject soundManagerGO = new GameObject("SoundManager");
                soundManager = new SoundManager(); // Create a new instance
                soundManager.Initialize(soundManagerGO); // Initialize SoundManager with a GameObject
            }
        }

        // Initialize UIManager (you can use FindObjectOfType or assign it manually in the Inspector)
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UiManager>();
            if (uiManager == null)
            {
                Debug.LogError("UIManager not found in the scene!");
            }
        }
    }

    // Method to pause the game
    public void TogglePause()
    {
        bool isPaused = Time.timeScale == 0f;
        Time.timeScale = isPaused ? 1f : 0f;  // Toggle time scale
        uiManager.OpenOptionsPanel(); // Show or hide the options panel
    }
}
