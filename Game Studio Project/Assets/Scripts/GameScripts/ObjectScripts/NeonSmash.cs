using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonSmash : MonoBehaviour
{    
    public GameObject neonSmashParticle;

    public int particleExplodeCount;
    private void OnCollisionEnter(Collision collision)
    {
        GameObject particles = Instantiate(neonSmashParticle);
        particles.transform.position = collision.contacts[0].point;
        particles.GetComponent<ParticleSystem>().Emit(particleExplodeCount);
        GameObject.Destroy(particles, 2);
    }
}
