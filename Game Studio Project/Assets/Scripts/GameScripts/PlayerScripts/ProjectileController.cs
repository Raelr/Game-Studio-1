using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the projectiles of the player
public class ProjectileController : InitialisedEntity
{
    [SerializeField]
    float timeBetweenShots = 2;


    float timeSinceLastShot;

    [SerializeField]
    GameObject projectile = null;
    [SerializeField]
    Transform spawnPos = null;

    public override void Initialise() {

        base.Initialise();
        timeSinceLastShot = 0;
    }

    // Fires projectiles form the player.
    //needs mouse position to know where to fire
    public void FireProjectile() {
        if ((Time.timeSinceLevelLoad - timeSinceLastShot) >= timeBetweenShots) {
            
            Instantiate(projectile, spawnPos.position, (projectile.transform.rotation * transform.rotation));
            timeSinceLastShot = Time.timeSinceLevelLoad;
        }
    }

}
