using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] music;
    public AudioClip[] sound;
    public AudioSource sourceMusic;
    public AudioSource sourceSFX;
    public float musicVolume;
    public float sfxVolume;
    public AudioClip defaultClip;



    private AudioClip GetSound(string clipName)
    {
        for (int i = 0; i < sound.Length; i++)
        {
            if (sound[i].name == clipName)
            {
                return sound[i];
            }
        }
        Debug.LogError("Can not find clip " + clipName);
        return defaultClip;
    }

    public void PlaySound(string clipName)
    {
        sourceSFX.PlayOneShot(GetSound(clipName), sfxVolume);
    }

    public void PlayMusic(int track)
    {
        sourceMusic.clip = music[track];
        sourceMusic.volume = musicVolume;
        sourceMusic.loop = true;
        sourceMusic.Play();
    }
}
