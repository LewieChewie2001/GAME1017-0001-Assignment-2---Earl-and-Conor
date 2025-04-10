using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Game.Instance == null || Game.Instance.SOMA == null) return;

        switch (scene.name)
        {
            case "StartScene":
                Game.Instance.SOMA.PlayMusic("StartTheme");
                break;
            case "LoseScene":
                Game.Instance.SOMA.PlayMusic("LoseTheme");
                break;
            case "GameScene":
                Game.Instance.SOMA.PlayMusic("I_Ran");
                break;
        }
    }
}
