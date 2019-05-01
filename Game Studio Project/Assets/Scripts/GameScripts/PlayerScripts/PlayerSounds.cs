using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : InitialisedEntity
{
    [Header("Player Audio Sources")]
    [SerializeField]
    AudioSource playerAudioConstant;

    [SerializeField]
    AudioSource playerAudioSingle;

    [Header("Constant Player Sounds")]
    [SerializeField]
    AudioClip playerConstantSound;

    [SerializeField]
    AudioClip playerImpactSound;

    public override void Initialise()
    {
        base.Initialise();

        playerAudioConstant = GetComponents<AudioSource>()[0];

        playerAudioSingle = GetComponents<AudioSource>()[1];
        
        PlayConstantSound();
    }

    public void PlaySingleSound()
    {

    }

    public void PlayConstantSound()
    {
        playerAudioConstant.Stop();
        playerAudioConstant.clip = playerConstantSound;
        playerAudioConstant.Play();
    }
}
