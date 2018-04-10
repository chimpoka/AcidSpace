using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    public Text score;
    public Text bestScore;
    public Image pauseMenu;
    public Image endGameMenu;

    private static HUD instance = null;

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
        Controller.mode = GameMode.Play;
        pauseMenu.gameObject.SetActive(false);
        endGameMenu.gameObject.SetActive(false);
        bestScore.text = "Best Score: " + Controller.bestScore.ToString();
    }

    private void Update()
    {
        score.text = "Now: " + Controller.score.ToString();
    }

    public void onPauseButtonClick()
    {
        pauseMenu.gameObject.SetActive(true);
        Controller.mode = GameMode.Pause;
    }

    public void onResumeButtonClick()
    {
        pauseMenu.gameObject.SetActive(false);
        Controller.mode = GameMode.Play;
    }

    public void onQuitButtonClick()
    {
        Application.Quit();
    }

    public void ShowEndGameMenu()
    {
        endGameMenu.gameObject.SetActive(true);
        Controller.mode = GameMode.Pause;
    }

    public void onNewGameButtonClick()
    {
        Controller.Instance.startFromCheckpoint = false;
        Controller.mode = GameMode.Play;
        SceneManager.LoadScene(0);  
    }

    public void onStartFromCheckpointClick()
    {
        Controller.Instance.startFromCheckpoint = true;
        Controller.mode = GameMode.Play;
        SceneManager.LoadScene(0);
    }

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

}
