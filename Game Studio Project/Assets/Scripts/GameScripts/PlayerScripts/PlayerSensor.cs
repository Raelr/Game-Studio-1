using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensor : InitialisedEntity
{
    public delegate void OnCollisionHandler();

	public delegate void OnNearMissHandler();

    public delegate void OnStunHandler();

    public OnCollisionHandler onCollision;

	public OnNearMissHandler onNearMiss;

    public OnStunHandler onStun;

	private bool hasCollided; 

    private void OnCollisionEnter(Collision collision) {
        
        if (collision.transform.tag == "Obstacle") {
			hasCollided = true;
            onCollision?.Invoke();
        }
        if (collision.transform.tag == "Stunner") {
            onStun?.Invoke();
        }
    }

	private void OnTriggerExit(Collider col) {

        
        if (col.transform.tag == "NeonRing")
            {
                Debug.Log("flown through a neon ring");
            }

        if (!hasCollided) {
			onNearMiss?.Invoke();
		}
		hasCollided = false;


	}
}
