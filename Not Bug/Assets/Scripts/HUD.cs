﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HUD : MonoBehaviour
{
    public GameObject tapToStart;
    public GameObject congratulationsWindow;
    public Image pauseMenu;
    public Image endGameMenu;
    public TextMeshProUGUI score;
    public TextMeshProUGUI bestScore;
    public TextMeshProUGUI life;
    public float fadingTime = 0.8f;

    private Fading fading;
    private static HUD instance = null;
    private Tutorial tutorial;

    public static HUD Instance
    {
        get
        {
            if (!instance) instance = new HUD();
            return instance;
        }
    }



    private void Awake()
    {
        //If there is already an instance of this class, then remove
        if (instance) { DestroyImmediate(this); return; }

        //Assign this instance as singleton
        instance = this;
    }

    private void Start()
    {
        //if (Controller.moveControl == MoveControl.Touchscreen)
        //{
        //    bestScore.text = "BEST SCORE: " + Controller.bestScoreTouchscreen.ToString();
        //    life.text = Controller.lifeTouchscreen.ToString();
        //}
        //else if (Controller.moveControl == MoveControl.Accelerometer)
        //{
        //    bestScore.text = "BEST SCORE: " + Controller.bestScoreAccelerometer.ToString();
        //    life.text = Controller.lifeAccelerometer.ToString();
        //}

        fading = gameObject.GetComponentInChildren<Fading>();
        fading.FadeIn(fadingTime);

        tutorial = GameObject.FindGameObjectWithTag("Player").GetComponent<Tutorial>();

        //if (tutorial.IsTouchscreenTutorial() == true)
        //    tutorial.FirstStartGameTouchscreen();
        //else if (tutorial.IsAccelerometerTutorial() == true)
        //    tutorial.FirstStartGameAccelerometer();
        //else
            StartGame();
    }

    public void SetScore(float value)
    {
        score.text = "NOW: " + value.ToString();
    }

    private void Update()
    {
        score.text = "NOW: " + Controller.score.ToString();

        if (!tutorial.IsTutorial())
        {
            if (Controller.gameMode == GameMode.Play && Controller.prepareToStartGame == true)
            {
                tapToStart.SetActive(true);
                if (tapToStart.activeSelf == true && Input.GetMouseButton(0) && Input.mousePosition.y < Screen.height * 0.81)
                {
                    tapToStart.SetActive(false);
                    Controller.prepareToStartGame = false;
                    GameObject.Find("ShipObject").GetComponent<RocketMobile>().StartGame();
                }
            }

            if (Controller.gameMode == GameMode.Pause)
            {
                tapToStart.SetActive(false);
            }
        }
    }



    private void StartGame()
    {
        pauseMenu.gameObject.SetActive(false);
        endGameMenu.gameObject.SetActive(false);

        if (Controller.moveControl == MoveControl.Touchscreen)
        {
            if (tutorial.IsTouchscreenTutorial())
                tutorial.FirstStartGameTouchscreen();
            bestScore.text = "BEST SCORE: " + Controller.bestScoreTouchscreen.ToString();
            life.text = Controller.lifeTouchscreen.ToString();
        }
        else if (Controller.moveControl == MoveControl.Accelerometer)
        {
            if (tutorial.IsAccelerometerTutorial())
                tutorial.FirstStartGameAccelerometer();
            bestScore.text = "BEST SCORE: " + Controller.bestScoreAccelerometer.ToString();
            life.text = Controller.lifeAccelerometer.ToString();
        }

        Controller.gameMode = GameMode.Play;
        Controller.prepareToStartGame = true;
        GameObject.Find("ShipObject").GetComponent<RocketMobile>().PrepareToStartGame();

        if (tutorial.IsTutorial() == false)
            tapToStart.SetActive(true);
    }



    // Pause menu

    public void onPauseButtonClick()
    {
        pauseMenu.gameObject.SetActive(true);
        Controller.gameMode = GameMode.Pause;
    }

    public void onResumeButtonClick()
    {
        pauseMenu.gameObject.SetActive(false);
        Controller.gameMode = GameMode.Play;
    }

    public void onQuitButtonClick()
    {
        Application.Quit();
    }


    // End Game menu

    public void ShowEndGameMenu()
    {
        endGameMenu.gameObject.SetActive(true);
        Controller.gameMode = GameMode.Pause;
    }

    public void onNewGameButtonClick()
    {
        Controller.Instance.startFromCheckpoint = false;
        if (Controller.moveControl == MoveControl.Touchscreen)
            Controller.lifeTouchscreen = 5;
        else if (Controller.moveControl == MoveControl.Accelerometer)
            Controller.lifeAccelerometer = 5;
        StartCoroutine("StartLevel");
    }

    public void onStartFromCheckpointClick()
    {
        Controller.Instance.startFromCheckpoint = true;
        StartCoroutine("StartLevel");
    }

    public void onMainMenuClick()
    {
        StartCoroutine("GoToMainMenu");
    }

    IEnumerator StartLevel()
    {
        fading.FadeOut(fadingTime);
        yield return new WaitForSeconds(fadingTime);
        fading.FadeIn(fadingTime);
        //GameObject.Find("ShipObject").GetComponent<RocketMobile>().StartGame();
        StartGame();
    }

    IEnumerator GoToMainMenu()
    {
        fading.FadeOut(fadingTime);
        yield return new WaitForSeconds(fadingTime);
        SceneManager.LoadScene(0);
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



    // Options menu

    public void onEffectsVolumeSlider(Slider slider)
    {
        Controller.Instance.GetComponent<AudioManager>().sourceEffects.volume = slider.value;
    }

    public void onMusicVolumeSlider(Slider slider)
    {
        Controller.Instance.GetComponent<AudioManager>().sourceMusic.volume = slider.value;
    }

    public void onOptionsClickUpdateEffectsSlider(Slider effectsSlider)
    {
        effectsSlider.value = Controller.Instance.GetComponent<AudioManager>().sourceEffects.volume;
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



    // Debug

    //public void onNewGameDebugClick()
    //{
    //    DataManager.SaveLoadNewGame();
    //}



    // Power ups

    public void onDecelerationButtonClick()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<RocketEffects>().isRocketSlow = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<RocketEffects>().slowScore = Controller.score;

       
    }

    public void onInvulnerabilityClick()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<RocketEffects>().isRocketInvulnerable = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<RocketEffects>().invulnerabilityScore = Controller.score;
    }



    public void ShowCongratulations()
    {
        congratulationsWindow.SetActive(true);
        StartCoroutine("WinGame");
    }

    public void HideCongratulations()
    {
        congratulationsWindow.SetActive(false);
    }

    IEnumerator WinGame()
    {
        fading.FadeOut(3);
        yield return new WaitForSeconds(3);
        HideCongratulations();
        SceneManager.LoadScene(0);       
    }

}
