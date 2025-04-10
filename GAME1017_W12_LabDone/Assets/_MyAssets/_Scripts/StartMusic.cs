using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusic : MonoBehaviour
{
    void Start()
    {
        Game.Instance.SOMA.PlayMusic("StartTheme");
    }
}
