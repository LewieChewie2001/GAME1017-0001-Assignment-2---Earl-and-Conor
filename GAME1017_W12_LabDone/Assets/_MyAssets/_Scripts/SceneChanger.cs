using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class SceneChanger : MonoBehaviour
{
    // This method will be called when the button is pressed
    public void ChangeScene(string sceneName)
    {
        // Load the scene by name
        SceneManager.LoadScene(sceneName);
    }
}
