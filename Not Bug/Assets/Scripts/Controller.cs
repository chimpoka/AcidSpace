using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode { Play, Pause}

public class Controller : MonoBehaviour
{
    static public GameMode mode;
    static public float score;
    static public float bestScore;
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
        //If there is already an instance of this class, then remove
        if (instance) { DestroyImmediate(this); return; }

        //Assign this instance as singleton
        instance = this;
        DontDestroyOnLoad(gameObject);
        DataManager.load();
    }

    private void Start()
    {
        GetComponent<AudioManager>().PlayMusic(track);
        HUD.Instance.ShowEndGameMenu();
    }

    private void Update()
    {
        score = Mathf.Floor(GameObject.FindGameObjectWithTag("Player").transform.position.x);
    }


}
