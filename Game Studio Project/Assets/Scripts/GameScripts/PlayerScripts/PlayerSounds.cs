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
    AudioClip playerConstantSound = null;

    [SerializeField]
    AudioClip playerImpactSound = null;

    const float maxVolumeAdjustment = 1f;

    public override void Initialise()
    {
        base.Initialise();

        playerAudioConstant = GetComponents<AudioSource>()[0];

        playerAudioSingle = GetComponents<AudioSource>()[1];

        playerAudioSingle.clip = playerImpactSound; 

        playerAudioSingle.pitch = Random.Range(0.8f, 1.2f);

        playerAudioSingle.Stop();

        PlayConstantSound();
    }

    public void PlayerImpactSound()
    {
        playerAudioSingle.Play();
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

        float volumeValue = percentage / 100 * maxVolumeAdjustment;

        playerAudioConstant.volume = volumeValue;
    }
}
