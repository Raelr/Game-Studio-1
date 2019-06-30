using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlternativeArchitecture;

public class PlayerSensor : InitialisedEntity
{
    public delegate void OnCollisionHandler();

    public delegate void OnNearMissHandler();
	
    public delegate void OnRingHitHandler();

    public delegate void OnRelicHitHandler();

    public delegate void OnPlayerCollectHandler();

    public OnCollisionHandler onCollision;
    public OnPlayerCollectHandler onPlayerCollect;
    public OnNearMissHandler onNearMiss;
    public OnRingHitHandler onRingHit;
    public OnRelicHitHandler onRelicHit;


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
            onRelicHit?.Invoke();
            StartCoroutine(NextLevelHit());
        }
        
    }

    IEnumerator NextLevelHit ()
    {
        for (int i = 0; i < 2; i++)
        {
            GameProgression.instance.NextLevel();
            yield return new WaitForSeconds(0.2f);
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
