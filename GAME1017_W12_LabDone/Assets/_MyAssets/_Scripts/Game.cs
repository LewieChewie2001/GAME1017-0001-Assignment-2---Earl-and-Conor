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
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("GameManager Awake on: " + gameObject.name);
        Initialize(); // ← THIS LINE WAS MISSING
    }


    private void Initialize()
    {
        SOMA = new SoundManager();
        SOMA.Initialize(gameObject);
        SOMA.AddSound("Jump", Resources.Load<AudioClip>("jump"), SoundManager.SoundType.SOUND_SFX);
        SOMA.AddSound("Roll", Resources.Load<AudioClip>("roll"), SoundManager.SoundType.SOUND_SFX);
        SOMA.AddSound("StillDre", Resources.Load<AudioClip>("StillDre"), SoundManager.SoundType.SOUND_MUSIC);
        SOMA.AddSound("I_Ran", Resources.Load<AudioClip>("I_Ran"), SoundManager.SoundType.SOUND_MUSIC);
        SOMA.AddSound("StartTheme", Resources.Load<AudioClip>("StartTheme"), SoundManager.SoundType.SOUND_MUSIC);
        SOMA.AddSound("LoseTheme", Resources.Load<AudioClip>("LoseTheme"), SoundManager.SoundType.SOUND_MUSIC);
        SOMA.PlayMusic("I_Ran");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If references are null, try to reassign them
        if (timerText == null)
            timerText = GameObject.Find("TimerText")?.GetComponent<TMP_Text>();

        if (bestTime == null)
            bestTime = GameObject.Find("BestTime")?.GetComponent<TMP_Text>();
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
