using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFire : MonoBehaviour
{

    public List<ParticleSystem> particleFires;
    public int particleFireCountMin, particleFireCountMax;

    public float particleEmitRate;
    private float particleEmitTimer;

    public AudioSource fireSound;
    public float firePitchMin, firePitchMax;
    
    void FixedUpdate()
    {

        particleEmitTimer += Time.deltaTime;

        if (Input.GetButtonDown("Fire1"))
        {
            if (particleEmitTimer > particleEmitRate)
            {
                particleEmitTimer = 0;
                foreach (ParticleSystem particle in particleFires)
                {
                    particle.Emit(Random.Range(particleFireCountMin, particleFireCountMax));
                }
                fireSound.pitch = Random.Range(firePitchMin, firePitchMax);
                fireSound.Play();
            }
        }

    }
}
