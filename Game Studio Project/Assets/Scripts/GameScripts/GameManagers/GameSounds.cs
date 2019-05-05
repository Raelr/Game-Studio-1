﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSounds : InitialisedEntity
{
    [Header("Audio Sources")]
    [SerializeField]
    AudioSource backgroundMusicSource;

    [Header("Clips")]
    [SerializeField]
    AudioClip backgroundMusic;

    public override void Initialise()
    {
        base.Initialise();

        backgroundMusicSource = GetComponent<AudioSource>();

        StartBackgroundMusic();
    }

    void StartBackgroundMusic()
    {
        backgroundMusicSource.Stop();
        backgroundMusicSource.clip = backgroundMusic;
        backgroundMusicSource.Play();
    }

    public void StopBackgroundMusic()
    {
        if (backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Stop();
        }
    }
}