using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] music;
    public AudioClip[] sound;
    public AudioSource sourceEffects;
    public AudioSource sourceMusic;
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
        sourceEffects.PlayOneShot(GetSound(clipName));
    }

    public void PlayMusic(int track)
    {
        sourceMusic.clip = music[track];
        sourceMusic.loop = true;
        sourceMusic.Play();
    }
}
