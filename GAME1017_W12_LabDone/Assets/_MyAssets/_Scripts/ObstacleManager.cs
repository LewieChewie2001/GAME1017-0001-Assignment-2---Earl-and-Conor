using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] List<GameObject> obstacles;
    [SerializeField] float moveSpeed;
    [SerializeField] Sprite[] sprites;

    private int obsCtr = 0; // Used to manage the gaps between obstacles.
    void Start()
    {
        obstacles = new List<GameObject>(); 
        for (int i = 0; i < 9; i++)
        {
            GameObject obsInst = GameObject.Instantiate(obstaclePrefab, new Vector3(i * 4f, -18.6f, 0f), Quaternion.identity);
            obsInst.transform.parent = transform;
            obstacles.Add(obsInst);
        }
        // Start the InvokeRepeating method.
        InvokeRepeating("MoveObstacles", 0f, Time.fixedDeltaTime);
    }
    private void MoveObstacles()
    {
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.transform.Translate(moveSpeed * Time.fixedDeltaTime, 0f, 0f);
        }
        if (obstacles[0].transform.position.x <= -4f)
        {
            // Remove the first obstacle.
            Destroy(obstacles[0]);
            obstacles.RemoveAt(0);
            float yPos = -22f;


            // 25% chance to spawn the obstacle higher
            if (Random.value < 0.25f)
            {
                yPos = -19f;
            }

            //respawns
            GameObject obsInst = GameObject.Instantiate(obstaclePrefab, new Vector3(32f, yPos, 0f), Quaternion.identity);

            if (obsCtr++ % 4 == 0)
            {
                obsInst.GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
                obsInst.AddComponent<BoxCollider2D>();
            }
            obsInst.transform.parent = transform;
            obsInst.layer = 4;
            obstacles.Add(obsInst);
        }
    }
}
