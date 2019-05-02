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

    const float maxVolumeAdjustment = 0.5f;

    const float minVolume = 0.01f;

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

    public void StopBackgroundSound()
    {
        if (playerAudioConstant.isPlaying)
        {
            playerAudioConstant.Stop();
        }
    }

    public void AdjustAudioSourceVolume(float volume)
    {
        float percentage = volume * 100;

        float volumeValue = percentage / 100 * 0.5f;

        playerAudioConstant.volume = volumeValue;
    }
}
