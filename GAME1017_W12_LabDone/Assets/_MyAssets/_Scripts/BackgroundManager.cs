using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private Transform[] backgrounds;
    [SerializeField] private Transform[] midgrounds;
    [SerializeField] private Transform[] foregrounds;
    [SerializeField] private float[] moveSpeeds; // [background, midground, foreground]

    private float[] sizes;
    private float[] backgroundStarts;
    private float[] midgroundStarts;
    private float[] foregroundStarts;

    void Start()
    {
        sizes = new float[3];
        sizes[0] = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
        sizes[1] = midgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
        sizes[2] = foregrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;

        backgroundStarts = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
            backgroundStarts[i] = backgrounds[i].transform.position.x;

        midgroundStarts = new float[midgrounds.Length];
        for (int i = 0; i < midgrounds.Length; i++)
            midgroundStarts[i] = midgrounds[i].transform.position.x;

        foregroundStarts = new float[foregrounds.Length];
        for (int i = 0; i < foregrounds.Length; i++)
            foregroundStarts[i] = foregrounds[i].transform.position.x;
    }

    void Update()
    {
        if (Time.timeScale > 0f)
        {
            MoveBackgrounds();
        }
    }

    private void MoveBackgrounds()
    {
        // Move all layers at their own speed
        foreach (var background in backgrounds)
            background.Translate(moveSpeeds[0] * Time.deltaTime, 0f, 0f);
        foreach (var midground in midgrounds)
            midground.Translate(moveSpeeds[1] * Time.deltaTime, 0f, 0f);
        foreach (var foreground in foregrounds)
            foreground.Translate(moveSpeeds[2] * Time.deltaTime, 0f, 0f);

        // Loop backgrounds
        if (backgrounds[0].position.x <= -sizes[0])
        {
            for (int i = 0; i < backgrounds.Length; i++)
                backgrounds[i].position = new Vector3(backgroundStarts[i], backgrounds[i].position.y, backgrounds[i].position.z);
        }

        if (midgrounds[0].position.x <= -sizes[1])
        {
            for (int i = 0; i < midgrounds.Length; i++)
                midgrounds[i].position = new Vector3(midgroundStarts[i], midgrounds[i].position.y, midgrounds[i].position.z);
        }

        if (foregrounds[0].position.x <= -sizes[2])
        {
            for (int i = 0; i < foregrounds.Length; i++)
                foregrounds[i].position = new Vector3(foregroundStarts[i], foregrounds[i].position.y, foregrounds[i].position.z);
        }
    }
}
