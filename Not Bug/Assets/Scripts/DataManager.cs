using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    static public void Save()
    {
        PlayerPrefs.SetInt("bestScoreTouchscreen", Controller.bestScoreTouchscreen);
        PlayerPrefs.SetInt("bestScoreAccelerometer", Controller.bestScoreAccelerometer);
        PlayerPrefs.SetInt("checkpointTouchscreen", Controller.checkpointTouchscreen);
        PlayerPrefs.SetInt("checkpointAccelerometer", Controller.checkpointAccelerometer);
        PlayerPrefs.SetInt("lifeTouchscreen", Controller.lifeTouchscreen);
        PlayerPrefs.SetInt("lifeAccelerometer", Controller.lifeAccelerometer);
        PlayerPrefs.SetInt("completedTouchscreen", Controller.completedTouchscreen);
        PlayerPrefs.SetInt("completedAccelerometer", Controller.completedAccelerometer);
        PlayerPrefs.SetFloat("effectsVolume", Controller.Instance.GetComponent<AudioManager>().sourceEffects.volume);
        PlayerPrefs.SetFloat("musicVolume", Controller.Instance.GetComponent<AudioManager>().sourceMusic.volume);
        PlayerPrefs.SetInt("track", Controller.Instance.track);
        PlayerPrefs.SetInt("showTutorialToggle", Controller.showTutorialToggle);
    }

    static public void Load()
    {
        Controller.bestScoreTouchscreen = PlayerPrefs.GetInt("bestScoreTouchscreen", 0);
        Controller.bestScoreAccelerometer = PlayerPrefs.GetInt("bestScoreAccelerometer", 0);
        Controller.checkpointTouchscreen = PlayerPrefs.GetInt("checkpointTouchscreen", 0);
        Controller.checkpointAccelerometer = PlayerPrefs.GetInt("checkpointAccelerometer", 0);
        Controller.lifeTouchscreen = PlayerPrefs.GetInt("lifeTouchscreen", 5);
        Controller.lifeAccelerometer = PlayerPrefs.GetInt("lifeAccelerometer", 5);
        Controller.completedTouchscreen = PlayerPrefs.GetInt("completedTouchscreen", 0);
        Controller.completedAccelerometer = PlayerPrefs.GetInt("completedAccelerometer", 0);
        Controller.Instance.GetComponent<AudioManager>().sourceEffects.volume = PlayerPrefs.GetFloat("effectsVolume", 0.5f);
        Controller.Instance.GetComponent<AudioManager>().sourceMusic.volume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        Controller.Instance.track = PlayerPrefs.GetInt("track", 0);
        Controller.showTutorialToggle = PlayerPrefs.GetInt("showTutorialToggle", 1);
    }

    static public void SaveNewGame()
    {
        PlayerPrefs.SetInt("bestScoreTouchscreen", 0);
        PlayerPrefs.SetInt("bestScoreAccelerometer", 0);
        PlayerPrefs.SetInt("checkpointTouchscreen", 0);
        PlayerPrefs.SetInt("checkpointAccelerometer", 0);
        PlayerPrefs.SetInt("lifeTouchscreen", 5);
        PlayerPrefs.SetInt("lifeAccelerometer", 5);
        PlayerPrefs.SetInt("completedTouchscreen", 0);
        PlayerPrefs.SetInt("completedAccelerometer", 0);
        PlayerPrefs.SetFloat("effectsVolume", 0.5f);
        PlayerPrefs.SetFloat("musicVolume", 0.5f);
        PlayerPrefs.SetInt("track", 0);
        PlayerPrefs.SetInt("showTutorialToggle", 1);
    }

}
