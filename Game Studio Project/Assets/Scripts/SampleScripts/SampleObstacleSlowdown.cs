using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlternativeArchitecture;

public class SampleObstacleSlowdown : MonoBehaviour
{

    public GamePooler pooler;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            SetObstacleSpeed(0); //multiplier of 0


        if (Input.GetKeyDown(KeyCode.X))
            SetObstacleSpeed(1); //multiplier of 0
    }


    void SetObstacleSpeed(float newSpeed)
    {
        List<GameObject> obstacles = pooler.GetObjects(ObjectType.OBSTACLE_SPHERE);
        foreach(GameObject obstacle in obstacles)
        {
            obstacle.GetComponent<Obstacle>().forceMultiplier = newSpeed;
            obstacle.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
