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
    private float bestRecordedTime = 0f;


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
        bestRecordedTime = PlayerPrefs.GetFloat("BestTime", 0f);
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
        // Reassign UI references
        if (timerText == null)
            timerText = GameObject.Find("TimerText")?.GetComponent<TMP_Text>();

        if (bestTime == null)
            bestTime = GameObject.Find("BestTime")?.GetComponent<TMP_Text>();

        if (bestTime != null && bestRecordedTime == 0f)
            bestTime.text = "Best Time: 0.00";

        if (scene.name != "Losing")
        {
            ResetTimer();
        }
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

        if (bestTime != null && bestRecordedTime == 0f)
            bestTime.text = "Best Time: 0.00";
    }


    public float GetCurrentTime()
    {
        return gameTime;
    }

    public void CheckAndUpdateBestTime()
    {
        float currentTime = GetCurrentTime();
        float savedBest = PlayerPrefs.GetFloat("BestTime", 0f);

        if (currentTime > savedBest)
        {
            PlayerPrefs.SetFloat("BestTime", currentTime);
            PlayerPrefs.Save(); // optional but ensures write

            bestRecordedTime = currentTime;

            if (UiManager.Instance != null)
                UiManager.Instance.UpdateBestTime(currentTime);
        }
    }

}
