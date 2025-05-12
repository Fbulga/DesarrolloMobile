using System;
using System.Collections;
using System.Collections.Generic;
using Enum;
using UnityEngine;
using UnityEngine.Serialization;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    [SerializeField] private SoundDB soundDB;
    [SerializeField] private AudioSource audioSourceBG;
    [SerializeField] private AudioSource audioSourceSFX;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    private void PlayLoopClip(SFXType clipType)
    {
        soundDB.TryGetAudioClip(clipType, out AudioClip audioClip);
        audioSourceBG.clip = audioClip;
        audioSourceBG.loop = true;
        audioSourceBG.Play();
    }
    
    public void PlayLoopableClip(SFXType clipType) => PlayLoopClip(clipType);
    
    
    private void PlaySFX(SFXType clipType)
    {
        soundDB.TryGetAudioClip(clipType, out AudioClip audioClip);
        audioSourceSFX.clip = audioClip;
        audioSourceSFX.loop = false;
        audioSourceSFX.Play();
    }
    public void PlaySFXClip(SFXType clipType) => PlaySFX(clipType);
    
}
