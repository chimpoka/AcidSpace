using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode { Play, Pause}
public enum MoveControl { Touchscreen, Accelerometer}

public class Controller : MonoBehaviour
{
    static public GameMode gameMode;
    static public MoveControl moveControl;
    static public bool prepareToStartGame;
    static public bool prepareToDie;
    static public int score;
    static public int bestScoreTouchscreen;
    static public int bestScoreAccelerometer;
    static public int currentScoreTouchscreen;
    static public int currentScoreAccelerometer;
    static public int checkpointTouchscreen;
    static public int checkpointAccelerometer;
    static public int lifeTouchscreen = 5;
    static public int lifeAccelerometer = 5;
    static public int completedTouchscreen = 0;
    static public int completedAccelerometer = 0;
    static public int showTutorialToggle = 1;
    static public int showTutorial = 1;
    public bool startFromCheckpoint = true;
    public int track = 0;
    

    private static Controller instance = null;

    public static Controller Instance
    {
        get
        {
            if (!instance) instance = new Controller();
            return instance;
        }
    }

    public static int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            HUD.Instance.SetScore(score);
        }
    }

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        DataManager.Load();
    }

    private void Start()
    {
        GetComponent<AudioManager>().PlayMusic(track);
    }

    private void Update()
    {
        if (gameMode == GameMode.Play)
            score = (int)Mathf.Floor(GameObject.FindGameObjectWithTag("Player").transform.position.x);
    }

    private void OnApplicationQuit()
    {
        DataManager.Save();
        //DataManager.SaveNewGame();
    }
}
