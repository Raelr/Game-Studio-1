using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensor : InitialisedEntity
{
    public delegate void OnCollisionHandler();

    public delegate void OnNearMissHandler();

    public OnCollisionHandler onCollision;

    public OnNearMissHandler onNearMiss;

    private bool hasCollided;

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.tag == "Obstacle")
        {
            hasCollided = true;
            onCollision?.Invoke();
        }
    }

	private void OnTriggerExit(Collider col) {

        if (col.gameObject.tag.Equals("Boost")) {
            if (!hasCollided) {
                onNearMiss?.Invoke();
            }

        }
        hasCollided = false;

    }
}
