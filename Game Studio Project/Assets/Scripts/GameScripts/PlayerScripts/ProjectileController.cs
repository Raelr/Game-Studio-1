﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the projectiles of the player
public class ProjectileController : InitialisedEntity
{
    [SerializeField]
    float timeBetweenShots = 2;


    float timeSinceLastShot;

    [SerializeField]
    GameObject projectile;

    public override void Initialise() {

        base.Initialise();
        timeSinceLastShot = 0;
    }

    // Fires projectiles form the player.
    //needs mouse position to know where to fire
    public void FireProjectile() {
        if ((Time.timeSinceLevelLoad - timeSinceLastShot) >= timeBetweenShots) {
            Instantiate(projectile, transform.position, projectile.transform.rotation);
            timeSinceLastShot = Time.timeSinceLevelLoad;
        }
    }

}
