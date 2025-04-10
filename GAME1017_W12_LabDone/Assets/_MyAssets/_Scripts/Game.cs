 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }
    public SoundManager SOMA;

    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text bestTime;

    private float gameTime = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Initialize();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Initialize()
    {
        SOMA = new SoundManager();
        SOMA.Initialize(gameObject);
        SOMA.AddSound("Jump", Resources.Load<AudioClip>("jump"), SoundManager.SoundType.SOUND_SFX);
        SOMA.AddSound("Roll", Resources.Load<AudioClip>("roll"), SoundManager.SoundType.SOUND_SFX);
        SOMA.AddSound("StillDre", Resources.Load<AudioClip>("StillDre"), SoundManager.SoundType.SOUND_MUSIC);
        SOMA.AddSound("I_Ran", Resources.Load<AudioClip>("I_Ran"), SoundManager.SoundType.SOUND_MUSIC);
        SOMA.PlayMusic("I_Ran");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Assign new references from the scene
        timerText = GameObject.Find("TimerText")?.GetComponent<TMP_Text>();
        bestTime = GameObject.Find("BestTime")?.GetComponent<TMP_Text>();

        if (timerText == null)
            Debug.LogWarning("TimerText not found in scene!");

        if (bestTime == null)
            Debug.LogWarning("BestTime not found in scene!");
    }

    private void Update()
    {
        if (Time.timeScale > 0f)
        {
            gameTime += Time.deltaTime;

            if (timerText != null)
                timerText.text = "Time: " + gameTime.ToString("F2") + "s";
        }
    }

    public void ResetTimer()
    {
        gameTime = 0f;
        if (timerText != null)
            timerText.text = "Time: 0.00s";
    }

    public float GetCurrentTime()
    {
        return gameTime;
    }
}
