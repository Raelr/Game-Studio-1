using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensor : InitialisedEntity
{
    public delegate void OnCollisionHandler();

    public OnCollisionHandler onCollision;

    private void OnCollisionEnter(Collision collision) {
        
        if (collision.transform.tag == "Obstacle") {
            onCollision?.Invoke();
        }
    }
}
