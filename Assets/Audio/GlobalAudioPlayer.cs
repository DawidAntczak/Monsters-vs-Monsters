using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudioPlayer : MonoBehaviour
{
    public static GlobalAudioPlayer Instance { get; private set; }
    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayOneShot(AudioClip audioClip, float volume=1f)
    {
        audioSource.PlayOneShot(audioClip, volume);
    }
}
