using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode { Play, Pause}
public enum MoveControl { Touchscreen, Accelerometer}

public class Controller : MonoBehaviour
{
    static public GameMode gameMode;
    static public MoveControl moveControl;
    static public float score;
    static public float bestScoreTouchscreen;
    static public float bestScoreAccelerometer;
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
            score = Mathf.Floor(GameObject.FindGameObjectWithTag("Player").transform.position.x);
    }

    private void OnApplicationQuit()
    {
        DataManager.Save();
    }
}
