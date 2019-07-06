using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //this functions are called as events in the animations
     [SerializeField] AudioSource reloadAudio;
     [SerializeField] AudioSource fastReloadAudio;
     [SerializeField] AudioSource shootAudio;
     [SerializeField] AudioSource emptyShootAudio;

    public void PlayReloadAudio()
    {
        reloadAudio.Play();
    }

    public void PlayFastReloadAudio()
    {
        fastReloadAudio.Play();
    }

    public void PlayShootAudio()
    {
        shootAudio.Play();
    }

    public void PlayEmptyShootAudio()
    {
        emptyShootAudio.Play();
    }
}
