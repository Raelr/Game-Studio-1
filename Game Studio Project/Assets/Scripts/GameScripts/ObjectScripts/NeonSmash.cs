using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonSmash : MonoBehaviour
{    
    public GameObject neonSmashParticle;

    public int particleExplodeCount, boostParticleCount = 30;

    public ParticleSystem boostEntryParticles;
    public AudioSource enterSound;



    private void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag.Equals("Boost"))
        {
            enterSound.pitch = Random.Range(0.9f, 1.2f);
            enterSound.Play();
            boostEntryParticles.Emit(boostParticleCount);
        }

    }

    private void OnTriggerExit(Collider col) {

        if (col.gameObject.tag.Equals("BoostAuto")) {

            HapticEngine.instance.Vibrate(HapticEffect.NEON_BOOST);


            GameObject particles = Instantiate(neonSmashParticle, transform);
            particles.transform.position = transform.position;
            particles.GetComponent<ParticleSystem>().Emit(particleExplodeCount);
            GameObject.Destroy(particles, 0.5f);

        }

    }
}
