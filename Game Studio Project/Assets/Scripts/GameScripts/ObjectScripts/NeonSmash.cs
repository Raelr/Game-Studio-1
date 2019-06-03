using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonSmash : MonoBehaviour
{    
    public GameObject neonSmashParticle;

    public int particleExplodeCount;



    	private void OnTriggerExit(Collider col) {

        if (col.gameObject.tag.Equals("BoostAuto")) {

            HapticEngine.instance.Vibrate(HapticEffect.NEON_BOOST);


            GameObject particles = Instantiate(neonSmashParticle, transform);
            particles.transform.position = transform.position;
            particles.GetComponent<ParticleSystem>().Emit(particleExplodeCount);
            GameObject.Destroy(particles, 0.4f);

        }

    }
}
