using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    static public void save()
    {
        PlayerPrefs.SetFloat("bestScore", Controller.bestScore);
    }

    static public void load()
    {
        Controller.bestScore = PlayerPrefs.GetFloat("bestScore", 0);
    }

}
