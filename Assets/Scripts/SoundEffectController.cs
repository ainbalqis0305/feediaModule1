using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectController : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayRainSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
    public void PlayCarSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    public void PlayLorrySound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
