using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensor : InitialisedEntity
{
    public delegate void OnCollisionHandler();

    public delegate void OnNearMissHandler();
	
    public delegate void OnRingHitHandler();

    public OnCollisionHandler onCollision;

    public OnNearMissHandler onNearMiss;
    public OnRingHitHandler onRingHit;


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
		
        if (col.gameObject.tag.Equals("BoostAuto")) {
            if (!hasCollided) {
                onRingHit?.Invoke();
            }
        }
        hasCollided = false;

    }
}
