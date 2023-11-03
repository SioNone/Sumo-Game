using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    [SerializeField]
    private AudioSource sfxPrefab;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    public void PlaySFX(AudioClip sfx, Transform sfxLocation, float volume)
    {
        AudioSource audioSource = Instantiate(sfxPrefab, sfxLocation.position, Quaternion.identity);

        audioSource.clip = sfx;

        audioSource.volume = volume;

        audioSource.Play();

        Destroy(audioSource, 5f);
    }
}
