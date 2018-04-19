using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    static public void Save()
    {
        PlayerPrefs.SetFloat("bestScoreTouchscreen", Controller.bestScoreTouchscreen);
        PlayerPrefs.SetFloat("bestScoreAccelerometer", Controller.bestScoreAccelerometer);
        PlayerPrefs.SetFloat("effectsVolume", Controller.Instance.GetComponent<AudioManager>().sourceEffects.volume);
        PlayerPrefs.SetFloat("musicVolume", Controller.Instance.GetComponent<AudioManager>().sourceMusic.volume);
        PlayerPrefs.SetInt("track", Controller.Instance.track);
    }

    static public void Load()
    {
        Controller.bestScoreTouchscreen = PlayerPrefs.GetFloat("bestScoreTouchscreen", 0);
        Controller.bestScoreAccelerometer = PlayerPrefs.GetFloat("bestScoreAccelerometer", 0);
        Controller.Instance.GetComponent<AudioManager>().sourceEffects.volume = PlayerPrefs.GetFloat("effectsVolume", 0.5f);
        Controller.Instance.GetComponent<AudioManager>().sourceMusic.volume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        Controller.Instance.track = PlayerPrefs.GetInt("track", 0);
    }

    static public void SaveLoadNewGame()
    {
        PlayerPrefs.SetFloat("bestScoreTouchscreen", 0);
        PlayerPrefs.SetFloat("bestScoreAccelerometer", 0);
        PlayerPrefs.SetFloat("effectsVolume", 0.5f);
        PlayerPrefs.SetFloat("musicVolume", 0.5f);
        PlayerPrefs.SetInt("track", 0);

        Controller.bestScoreTouchscreen = 0;
        Controller.bestScoreAccelerometer = 0;
        Controller.Instance.GetComponent<AudioManager>().sourceEffects.volume = 0.5f;
        Controller.Instance.GetComponent<AudioManager>().sourceMusic.volume = 0.5f;
        Controller.Instance.track = 0;
    }

}
