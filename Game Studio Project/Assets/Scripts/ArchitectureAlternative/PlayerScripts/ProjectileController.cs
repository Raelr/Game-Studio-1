using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the projectiles of the player
public class ProjectileController : InitialisedEntity
{
    public override void Initialise() {

        base.Initialise();
    }

    // Fires projectiles form the player.
    public void FireProjectile() {

        Debug.LogWarning("Fire!");
    }
}
