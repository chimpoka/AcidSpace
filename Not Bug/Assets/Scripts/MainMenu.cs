using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public float fadingTime = 0.8f;

    private Fading fading;

    private void Start()
    {
        Controller.gameMode = GameMode.Pause;
        fading = gameObject.GetComponentInChildren<Fading>();
        fading.FadeIn(fadingTime);

        RectTransform[] rectTransforms = GetComponentsInChildren<RectTransform>(true);
        foreach (RectTransform rt in rectTransforms)
        {
            if (rt.name == "MainMenu")
                rt.gameObject.SetActive(true);
            if (rt.name == "PlayMenu")
                rt.gameObject.SetActive(false);
            if (rt.name == "OptionsMenu")
                rt.gameObject.SetActive(false);
        }
    }

    public void onQuitClick()
    {
        Application.Quit();
    }



    // Play menu

    public void onTouchscreenButtonClick()
    {
        Controller.moveControl = MoveControl.Touchscreen;
        Controller.Instance.startFromCheckpoint = true;
        StartCoroutine("ChangeScene", SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void onAccelerometerButtonClick()
    {
        Controller.moveControl = MoveControl.Accelerometer;
        Controller.Instance.startFromCheckpoint = true;
        StartCoroutine("ChangeScene", SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator ChangeScene(int scene)
    {
        fading.FadeOut(fadingTime);
        yield return new WaitForSeconds(fadingTime);
        SceneManager.LoadScene(scene);
    }



    // Options menu

    public void onEffectsVolumeSlider(Slider slider)
    {
        Controller.Instance.GetComponent<AudioManager>().sourceEffects.volume = slider.value;
    }

    public void onOptionsClickUpdateEffectsSlider(Slider effectsSlider)
    {
        effectsSlider.value = Controller.Instance.GetComponent<AudioManager>().sourceEffects.volume;
    }

    public void onMusicVolumeSlider(Slider slider)
    {
        Controller.Instance.GetComponent<AudioManager>().sourceMusic.volume = slider.value;
    } 

    public void onOptionsClickUpdateMusicSlider(Slider musicSlider)
    {      
        musicSlider.value = Controller.Instance.GetComponent<AudioManager>().sourceMusic.volume;
    }

    public void onTrackSlider(Slider slider)
    {
        AudioManager audio = Controller.Instance.GetComponent<AudioManager>();

        float segmentLength = 1.0f / audio.music.Length;
        float segment = 0;
        int track = -1;
        int prevTrack = Controller.Instance.track;

        do
        {
            segment += segmentLength;
            track++;
        }
        while (slider.value >= segment);

        slider.value = segment - segmentLength / 1000;

        if (track != prevTrack)
        {          
            Controller.Instance.track = track;
            audio.PlayMusic(Controller.Instance.track);
        }
    }

    public void onOptionsClickUpdateTrackSlider(Slider trackSlider)
    {
        AudioManager audio = Controller.Instance.GetComponent<AudioManager>();
        float segmentLength = 1.0f / audio.music.Length;

        trackSlider.value = Controller.Instance.track * segmentLength + segmentLength - segmentLength / 1000;
    }



    // Music

    public void onNextTrackClick()
    {
        AudioManager audio = Controller.Instance.GetComponent<AudioManager>();


        if (Controller.Instance.track >= audio.music.Length - 1)
            Controller.Instance.track = 0;
        else
            ++Controller.Instance.track;

        audio.PlayMusic(Controller.Instance.track);
    }

    public void onPreviousTrackClick()
    {
        AudioManager audio = Controller.Instance.GetComponent<AudioManager>();

        if (Controller.Instance.track == 0)
            Controller.Instance.track = audio.music.Length - 1;
        else
            --Controller.Instance.track;

        audio.PlayMusic(Controller.Instance.track);
    }



    // Debug

    public void onNewGameDebugClick()
    {
        DataManager.SaveLoadNewGame();
    }
}
