using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    // UI elements
    public GameObject optionsPanel;  // Reference to the options panel
    public Slider soundSlider;       // Reference to the sound volume slider
    [SerializeField] TMP_Text timerText;           // Timer display text
    [SerializeField] TMP_Text bestTimeText;        // Best time display text
    [SerializeField] TMP_Text livesText;           // Lives display text
    private float gameTime = 0f;     // Timer for the game
    private int lives = 3;           // Example lives system

    private void Start()
    {
        // Initialize the UI elements, e.g., setting the sound slider to current volume
        soundSlider.value = AudioListener.volume;
        soundSlider.onValueChanged.AddListener(OnSoundSliderChanged);
    }

    private void Update()
    {
        if (Time.timeScale == 1f) // Only update timer if the game is not paused
        {
            gameTime += Time.deltaTime;
            timerText.text = "Time: " + gameTime.ToString("F2");
        }
    }

    // Toggle the visibility of the options panel
    public void ToggleOptionsPanel(bool isPaused)
    {
        optionsPanel.SetActive(isPaused);
    }

    // Handle sound slider change
    private void OnSoundSliderChanged(float value)
    {
        // Update the sound in SoundManager
        MainController.Instance.soundManager.SetVolume(value);
    }

    // Update lives and best time display (add any game-specific logic here)
    public void UpdateLives(int newLives)
    {
        lives = newLives;
        livesText.text = "Lives: " + lives;
    }

    public void UpdateBestTime(float bestTime)
    {
        bestTimeText.text = "Best Time: " + bestTime.ToString("F2");
    }
}
