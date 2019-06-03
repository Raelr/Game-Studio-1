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
            HapticEngine.instance.Vibrate(HapticEffect.HIT);
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

        //Debug.Log(col.gameObject.tag);

        if (col.gameObject.tag.Equals("BoostAuto")) {
            if (!hasCollided) {
                onRingHit?.Invoke();
            }
        }
        hasCollided = false;

    }
}
