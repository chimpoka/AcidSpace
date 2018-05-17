using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorialWindow;
    private GameObject howToFlyTouchscreen;
    private GameObject howToFlyAccelerometer;
    private GameObject checkpoint;
    private GameObject life;
    private bool checkpointChecked = false;
    private bool lifeCheck = false;

    private void GetWindows()
    {
        if (IsTutorial())
        {
            Transform[] transforms = tutorialWindow.GetComponentsInChildren<Transform>(true);
            foreach (Transform tr in transforms)
            {
                if (tr.name == "HowToFlyTouchscreen")
                    howToFlyTouchscreen = tr.gameObject;
                else if (tr.name == "HowToFlyAccelerometer")
                    howToFlyAccelerometer = tr.gameObject;
                else if (tr.name == "Checkpoint")
                    checkpoint = tr.gameObject;
                else if (tr.name == "Life")
                    life = tr.gameObject;
            }
        }
    }

    public void FirstStartGameTouchscreen()
    {
        GetWindows();
       // GetComponent<RocketMobile>().PrepareToStartGame();
        howToFlyTouchscreen.SetActive(true);
       // Controller.prepareToStartGame = true;
        //Controller.gameMode = GameMode.Play;
        checkpointChecked = false;
        lifeCheck = false;
}

    public void FirstStartGameAccelerometer()
    {
        GetWindows();
        //GetComponent<RocketMobile>().PrepareToStartGame();
        howToFlyAccelerometer.SetActive(true);
        //Controller.prepareToStartGame = true;
        //Controller.gameMode = GameMode.Play;
        checkpointChecked = false;
        lifeCheck = false;
    }

    private void Update()
    {   
        if (IsTutorial())
        {
            if (IsTouchscreenTutorial())
            {
                if (Controller.gameMode == GameMode.Play && Controller.prepareToStartGame == true)
                {
                    howToFlyTouchscreen.SetActive(true);
                    if (howToFlyTouchscreen.activeSelf == true && Input.GetMouseButtonDown(0) && Input.mousePosition.y < Screen.height * 0.81)
                    {
                        howToFlyTouchscreen.SetActive(false);
                        Controller.prepareToStartGame = false;
                        GetComponent<RocketMobile>().StartGame();
                    }
                }
                else if (Controller.gameMode == GameMode.Pause)
                {
                    howToFlyTouchscreen.SetActive(false);
                }

            }

            if (IsAccelerometerTutorial())
            {
                if (Controller.gameMode == GameMode.Play && Controller.prepareToStartGame == true)
                {
                    howToFlyAccelerometer.SetActive(true);
                    if (howToFlyAccelerometer.activeSelf == true && Input.GetMouseButtonDown(0) && Input.mousePosition.y < Screen.height * 0.81)
                    {
                        howToFlyAccelerometer.SetActive(false);
                        Controller.prepareToStartGame = false;
                        GetComponent<RocketMobile>().StartGame();
                    }
                }
                else if (Controller.gameMode == GameMode.Pause)
                {
                    howToFlyAccelerometer.SetActive(false);
                }
            }

            //Debug.Log("Score: " + Controller.Score);
            //Debug.Log("Check: " + checkpointChecked);
            if (Controller.Score <= 0)
                Controller.Score = 0;
            else if (checkpointChecked == false)
            {
                GetComponent<RocketMobile>().StopGame();
                checkpoint.SetActive(true);
                if (checkpoint.activeSelf == true && Input.GetMouseButtonDown(0) && Input.mousePosition.y < Screen.height * 0.81)
                {
                    checkpoint.SetActive(false);
                    checkpointChecked = true;
                    GetComponent<RocketMobile>().StartGame();
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsTutorial() && lifeCheck == false)
        {
            life.SetActive(true);
            GetComponent<RocketMobile>().StopGame();
            if (life.activeSelf == true && Input.GetMouseButtonDown(0) && Input.mousePosition.y < Screen.height * 0.81)
            {
                life.SetActive(false);
                if (checkpointChecked == true)
                {
                    if (IsTouchscreenTutorial())
                        Controller.firstPlayTouchscreen = 0;
                    else if (IsAccelerometerTutorial())
                        Controller.firstPlayAccelerometer = 0;
                }
                //else
                //{
                //    if (IsTouchscreenTutorial())
                //        FirstStartGameTouchscreen();
                //    else if (IsAccelerometerTutorial())
                //        FirstStartGameAccelerometer();
                //}
                lifeCheck = true;
                GetComponent<RocketMobile>().Die();
                Debug.Log(Controller.firstPlayTouchscreen);
            }
        }
    }

    

    public bool IsTutorial()
    {
        if ((Controller.firstPlayTouchscreen == 1 && Controller.moveControl == MoveControl.Touchscreen)
       || (Controller.firstPlayAccelerometer == 1 && Controller.moveControl == MoveControl.Accelerometer))
            return true;
        else
            return false;
    }

    public bool IsTouchscreenTutorial()
    {
        if (Controller.firstPlayTouchscreen == 1 && Controller.moveControl == MoveControl.Touchscreen)
            return true;
        else
            return false;
    }

    public bool IsAccelerometerTutorial()
    {
        if (Controller.firstPlayAccelerometer == 1 && Controller.moveControl == MoveControl.Accelerometer)
            return true;
        else
            return false;
    }

}
