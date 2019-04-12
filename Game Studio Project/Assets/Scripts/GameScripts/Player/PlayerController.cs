using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// the player controller acts as the primary API for the ship
// e.g. if the ship smashes, we would call 'Smash()' on the controller
// and the controller will then run the respective scripts required e.g.
// PlayerMaster.Data.SetAlive(false)
// PlayerMaster.Physics.SmashPhysics()
// PlayerMaster.Effects.SmashEffect()

public class PlayerController : MonoBehaviour
{
    private PlayerMaster playerMaster;

    public PlayerMaster PlayerMaster
    {
        get { return playerMaster; }
        set { playerMaster = value; }
    }
    
    public void SetupPlayerMasterReference (PlayerMaster master)
    {
        PlayerMaster = master;
    }


    public void InitialiseAll()
    {
        // initialises all the scripts attached to the spaceship
        PlayerMaster.Input.Initialise();
        PlayerMaster.Effects.Initialise();
        
    }

    

    // Player Controller API Functions


    public void Fire ()
    {
        Debug.Log(" fire! ");
        //e.g. spawn bullet via GameObject spawnedBullet = PlayerMaster.GameMaster.Spawner.SpawnBullet();
        //PlayerMaster.Effects.BulletFireEffect()
        
    }


    public void Smash ()
    {

        // run respective scripts on the ship e.g.
        // PlayerMaster.Data.SetAlive(false)
        // PlayerMaster.Physics.SmashPhysics()
        // PlayerMaster.Effects.SmashEffect()

    }

    public void Move(Vector2 velocity) {



    }


}
