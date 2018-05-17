using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMobile : MonoBehaviour
{
    [Header("Common Settings")]
    public ParticleSystem particleExplosion;
    public ParticleSystem particleCheckpoint;
    public float startSpeed =12f;
    public float startPosY = 8.2f;
    public float maxAngle = 45f;
    public int checkPoint = 200;
   


    [Header("TouchPad Movement Settings")]
    public float rotationTime = 2f;

    [Header("Accelerometer Movement Settings")]
    public float rotationSmoothing = 0.1f;
    public float rotationPower = 2f;

    [Header("Debug")]
    public float startPosX;
 

    private int changeEnvironment = 100;
    private bool checkPointDone = false;
    private bool mousePressed = false;
    private float rotationDeltaTime = 0.0f;
    private float speedPerFrame_y = 0;
    private float bestScore;
    private float speed;
    private int currentCheckPoint;
    private int life;




    void Start()
    {
       // StartGame();

        changeEnvironment = GameObject.Find("Environment").GetComponent<Environment>().changeEnvironmentPoint;
    }

    void Update()
    {
        if (Controller.gameMode == GameMode.Play && speed > 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                HUD.Instance.onPauseButtonClick();           

            if ((Mathf.Floor(transform.position.x) % changeEnvironment == 0) && (Mathf.Floor(transform.position.x) != 0))
                GameObject.Find("Environment").GetComponent<Environment>().ChangeEnvironment();             
            
            if (Controller.moveControl == MoveControl.Touchscreen)
                MoveTouchscreen();
            else if (Controller.moveControl == MoveControl.Accelerometer)
                MoveAccelerometer();

            CheckPoint();
        }
    }

   

    private void OnTriggerStay(Collider other)
    {
        if (GetComponent<Tutorial>().IsTutorial() == false)
        {
            if ((Controller.gameMode == GameMode.Play) && (GetComponent<RocketEffects>().isRocketInvulnerable == false))
                Die();
        }
    }




    // User Methods

    public void PrepareToStartGame()
    {
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>(true);

        foreach (Transform tr in transforms)
        {
            if (tr.name == "Ship")
                tr.gameObject.SetActive(true);
            else if (tr.name == "Tail")
                tr.gameObject.SetActive(true);
        }

        if (Controller.moveControl == MoveControl.Touchscreen)
        {
            bestScore = Controller.bestScoreTouchscreen;
            currentCheckPoint = Controller.checkpointTouchscreen;
            life = Controller.lifeTouchscreen;
        }
        else if (Controller.moveControl == MoveControl.Accelerometer)
        {
            bestScore = Controller.bestScoreAccelerometer;
            currentCheckPoint = Controller.checkpointAccelerometer;
            life = Controller.lifeAccelerometer;
        }

        if (Controller.Instance.startFromCheckpoint == true)
        {
            #if UNITY_EDITOR
                        transform.position = new Vector3(startPosX, startPosY, transform.position.z);
                        #endif
            #if !UNITY_EDITOR
                transform.position = new Vector3(currentCheckPoint / checkPoint * checkPoint, startPosY, transform.position.z);
            #endif
        }
        else
            transform.position = new Vector3(0, startPosY, transform.position.z);

        if (GetComponent<Tutorial>().IsTutorial())
            transform.position = new Vector3(-30, startPosY, transform.position.z);
        else if (life == 5)
            transform.position = new Vector3(0, startPosY, transform.position.z);

        transform.eulerAngles = Vector3.zero;
        speedPerFrame_y = 0;
        mousePressed = false;

        GetComponent<RocketEffects>().isRocketSlow = false;
        GetComponent<RocketEffects>().isRocketInvulnerable = false;
        GetComponent<RocketEffects>().shield.SetActive(false);
    }

    public void StartGame()
    {
        Time.timeScale = 1.0f;
        speed = startSpeed;
    }

    public void StopGame()
    {
        speed = 0;
    }

    private void CheckPoint()
    {
        if (transform.position.x >= 2000)
        {
            if (Controller.moveControl == MoveControl.Touchscreen)
            {
                Controller.completedTouchscreen = 1;
            }
            else if (Controller.moveControl == MoveControl.Accelerometer)
            {
                Controller.completedAccelerometer = 1;
            }

            GetComponent<RocketEffects>().isRocketSlow = true;
            GetComponent<RocketEffects>().slowScore = Controller.score;

            if (transform.position.x >= 2020)
                HUD.Instance.ShowCongratulations();
        }

        if ((Mathf.Floor(transform.position.x) % checkPoint == 0) && (transform.position.x != 0) && (checkPointDone == false))
        {
            checkPointDone = true;

            Animator[] animators = GetComponentsInChildren<Animator>();
            foreach (Animator anim in animators)
            {
                if (anim.name == "Ship")
                    anim.SetTrigger("CheckPoint");
            }

            particleCheckpoint.Play();
            Controller.Instance.GetComponent<AudioManager>().PlaySound("CheckPoint");
        }
        else if (Mathf.Floor(transform.position.x) % checkPoint != 0)
        {
            checkPointDone = false;
        }
    }   

    public void Die()
    {
        speed = 0;
        life--;

        if (Controller.moveControl == MoveControl.Touchscreen)
        {
            if (life == 0)
            {
                Controller.checkpointTouchscreen = 0;
                Controller.lifeTouchscreen = 5;
            }
            else
            {
                Controller.checkpointTouchscreen = Controller.score;
                Controller.lifeTouchscreen = life;
            }
        }
        else if (Controller.moveControl == MoveControl.Accelerometer)
        {
            if (life == 0)
            {
                Controller.checkpointAccelerometer = 0;
                Controller.lifeAccelerometer = 5;
            }
            else
            {
                Controller.checkpointAccelerometer = Controller.score;
                Controller.lifeAccelerometer = life;
            }
        }

        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform tr in transforms)
        {
            if (tr.name == "Ship")
                tr.gameObject.SetActive(false);
            else if (tr.name == "Tail")
                tr.gameObject.SetActive(false);
        }

        if (Controller.score > bestScore)
        {
            if (Controller.moveControl == MoveControl.Touchscreen)
                Controller.bestScoreTouchscreen = Controller.score;
            else if (Controller.moveControl == MoveControl.Accelerometer)
                Controller.bestScoreAccelerometer = Controller.score;
        }
            

        particleExplosion.Play();
        Controller.Instance.GetComponent<AudioManager>().PlaySound("Explosion");
        GetComponent<RocketEffects>().isRocketInvulnerable = false;

        HUD.Instance.ShowEndGameMenu();
        Controller.gameMode = GameMode.Pause;
    }



    private void MoveAccelerometer()
    {
        // Поворот телефона влево-вправо
        float AngleY = Mathf.Atan2(Input.acceleration.x, -Input.acceleration.y) * Mathf.Rad2Deg * -1;

        // При повороте телефона на X градусов, в игре самолет поворачивается на X * rotationPower градусов
        AngleY *= rotationPower;

        // Угол поворота в игре от -maxAngle до +maxAngle
        float RotZ = Mathf.Clamp(AngleY, -maxAngle, maxAngle);

        float eulerAngleZ = transform.eulerAngles.z;
        // Перевод из 355 градусов в -5
        if (eulerAngleZ > maxAngle)
            eulerAngleZ -= 360;
     
        // Плавный поворот от текущего угла к RotZ
        RotZ = Mathf.Lerp(eulerAngleZ, RotZ, rotationSmoothing);

        // Скорость по оси Y выражаем через угол поворота самолета
        float speedPerFrame_y = speed * Mathf.Tan(RotZ * Mathf.Deg2Rad);

        Vector3 deltaPos = new Vector3(speed, speedPerFrame_y, 0);

        transform.position += deltaPos.normalized * speed * Time.deltaTime;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, RotZ);
    }

    private void MoveTouchscreen()
    {
        Vector3 newRot = transform.eulerAngles;

        float eulerAngleZ = transform.eulerAngles.z;
        // Перевод из 355 градусов в -5
        if (eulerAngleZ > maxAngle * 1.1)
            eulerAngleZ -= 360;

        if (Input.GetMouseButton(0) && Input.mousePosition.y < Screen.height * 0.81)
        {            
            if (mousePressed == false)
            {
                mousePressed = true;
                rotationDeltaTime = 0;
            }

            // Плавный поворот
            if (rotationDeltaTime < rotationTime)
            {
                newRot.z = Mathf.Lerp(eulerAngleZ, maxAngle, rotationDeltaTime / rotationTime);
               // Debug.Log(speedPerFrame_y + ", " + speed + ", " + rotationDeltaTime / rotationTime);
                speedPerFrame_y = Mathf.Lerp(speedPerFrame_y, speed, rotationDeltaTime / rotationTime);
                rotationDeltaTime += Time.deltaTime;
            }
            else
            {
                newRot.z = maxAngle;
            }
        }
        else
        {
            if (mousePressed == true)
            {
                mousePressed = false;
                rotationDeltaTime = 0;
            }

            // Плавный поворот
            if (rotationDeltaTime < rotationTime)
            {
                newRot.z = Mathf.Lerp(eulerAngleZ, -maxAngle, rotationDeltaTime / rotationTime);
                speedPerFrame_y = Mathf.Lerp(speedPerFrame_y, -speed, rotationDeltaTime / rotationTime);
                rotationDeltaTime += Time.deltaTime;
            }
            else
            {
                newRot.z = -maxAngle;
            }          
        }
        Vector3 deltaPos = new Vector3(speed, speedPerFrame_y, transform.position.z);

        transform.position += deltaPos.normalized * speed * Time.deltaTime;
        transform.eulerAngles = newRot;
    }
}
