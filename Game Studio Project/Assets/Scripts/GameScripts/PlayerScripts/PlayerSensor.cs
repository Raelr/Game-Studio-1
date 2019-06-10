using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensor : InitialisedEntity
{
    public delegate void OnCollisionHandler();

    public delegate void OnNearMissHandler();
	
    public delegate void OnRingHitHandler();

    public delegate void OnPlayerCollectHandler();

    public OnCollisionHandler onCollision;
    public OnNearMissHandler onNearMiss;
    public OnRingHitHandler onRingHit;
    public OnPlayerCollectHandler onPlayerCollect;


    private bool hasCollided;

    public ParticleSystem boostCrashParticle;
    public int boostCrashParticleCount = 20;


    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.transform.tag;
        if (tag == "Obstacle" || tag == "ObstacleBoost")
        {
            if (tag == "ObstacleBoost")
            {
                boostCrashParticle.Emit(boostCrashParticleCount);
            }
            HapticEngine.instance.Vibrate(HapticEffect.HIT);
            hasCollided = true;
            onCollision?.Invoke();
        }

        if (tag == "Collectable") {

            onPlayerCollect?.Invoke();
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
