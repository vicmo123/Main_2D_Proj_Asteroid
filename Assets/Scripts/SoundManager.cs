using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    #region Singleton
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
                instance = new SoundManager();
            return instance;
        }
    }

    private SoundManager() { }
    #endregion

    private GameObject soundManager;

    private AudioSource winSound;
    private AudioSource looseSound;
    private AudioSource mainMusic;
    private AudioSource shootSound;
    private AudioSource collisionSound;
    private AudioSource clickSound;
    private AudioSource explosionSound;

    public void Initialize()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager");

        AudioSource[] tabAudioSource = soundManager.GetComponents<AudioSource>();

        winSound = tabAudioSource[0];
        looseSound = tabAudioSource[1];
        mainMusic = tabAudioSource[2];
        shootSound = tabAudioSource[3];
        collisionSound = tabAudioSource[4];
        clickSound = tabAudioSource[5];
        explosionSound = tabAudioSource[6];
    }

    public void PlayWinSound()
    {
        winSound.Play();
    }

    public void PlayLooseSound()
    {
        looseSound.Play();
    }

    public void PlayMainMusic()
    {
        mainMusic.Play();
    }

    public void PlayShootSound()
    {
        shootSound.Play();
    }

    public void PlayCollisionSound()
    {
        collisionSound.Play();
    }

    public void PlayClickSound()
    {
        clickSound.Play();
    }

    public void PlayExplosionSound()
    {
        explosionSound.Play();
    }
}
