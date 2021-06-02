using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SausageRain))]
[RequireComponent(typeof(SausageSplash))]
public class SausageBoss : Boss
{
    private SausageRain sausageRain;
    private SausageSplash sausageSplash;
    private SausageTentaclePhase sausageTentaclePhase;
    [SerializeField] public AudioClip bossFightMusicPrefab;
    private AudioSource audioSource;
    private float fightMusicVolume = 0.04f;


    new void Awake()
    {
        base.Awake();
        sausageRain = GetComponent<SausageRain>();
        sausageSplash = GetComponent<SausageSplash>();
        sausageTentaclePhase = GetComponent<SausageTentaclePhase>();
        audioSource = GetComponent<AudioSource>();
    }

    new void Start()
    {
        base.Start();
        FindObjectOfType<MusicPlayer>().Pause();
        audioSource.clip = bossFightMusicPrefab;
        audioSource.volume = fightMusicVolume;
        audioSource.Play();
    }
    
    new void Update()
    {
        if (!GamePause.isPaused)
        {
            base.Update();
            if (arrived)
            {
                if (currentPhase >= 1)
                {
                    sausageRain.Attack();
                }
                if (currentPhase >= 2)
                {
                    sausageSplash.Attack();
                }
                sausageTentaclePhase.Attack(currentPhase);
            }
        }
    }

    public override void Die()
    {
        audioSource.Stop();
        FindObjectOfType<MusicPlayer>().Resume();
        base.Die();
    }

    public void StopAnimations()
    {
        animator.SetBool("Hit", false);
        animator.SetBool("Shake", false);
    }
}
