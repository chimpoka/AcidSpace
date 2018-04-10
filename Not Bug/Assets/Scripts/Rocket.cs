using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    public ParticleSystem particleExplosion;
    public ParticleSystem particleCheckpoint;
    public float speedPerFrame_x = 0.2f;
    public float speedPerFrame_y = 0.2f;
    public float rotationSpeed;
    Vector3 newPos;
    Vector3 newRot;
    float rotationTime = 0.0f;
    bool mousePressed = true;
    private float maxAngle;
    private float accel_y;
    public int checkPoint = 200;
    private bool repeat = false;
    
    void Start ()
    {
        maxAngle = Mathf.Atan2(speedPerFrame_y, speedPerFrame_x) / Mathf.PI * 180;
        accel_y = 0;

        if (Controller.Instance.startFromCheckpoint == true)
            transform.position = new Vector3((int)Controller.bestScore / checkPoint * checkPoint, transform.position.y, transform.position.z);
        else
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        
        Debug.Log("maxAngle: " + maxAngle);
    }
	
	void Update ()
    {
        if (Controller.mode == GameMode.Play)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HUD.Instance.onPauseButtonClick();
            }

            Move();
            if ((Mathf.Floor(transform.position.x) % 200 == 0) && (repeat == false))
            {
                Debug.Log("CheckPoint");
                repeat = true;
                Debug.Log(repeat + "   " + Mathf.Floor(transform.position.x));
                GetComponentInChildren<Animator>().SetTrigger("CheckPoint");
                particleCheckpoint.Play();
                Controller.Instance.GetComponent<AudioManager>().PlaySound("CheckPoint");          
            }
            else if (Mathf.Floor(transform.position.x) % 200 == 0)
            {
                repeat = true;
            }
            else
            {
                repeat = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Controller.mode == GameMode.Play)
            Die();
    }

    private void Die()
    {
        speedPerFrame_x = 0;
        speedPerFrame_y = 0;
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform tr in transforms)
        {
            if (tr.name == "Ship")
                tr.gameObject.SetActive(false);
            else if (tr.name == "Tail")
                tr.gameObject.SetActive(false);
        }

        if (Controller.score > Controller.bestScore)
            Controller.bestScore = Controller.score;

        particleExplosion.Play();
        Controller.Instance.GetComponent<AudioManager>().PlaySound("Explosion");
        HUD.Instance.ShowEndGameMenu();
        Controller.mode = GameMode.Pause;
        DataManager.save();
    }

    private void Move()
    {
        newPos = new Vector3(transform.position.x + speedPerFrame_x, transform.position.y, transform.position.z);
        newRot = transform.eulerAngles;

        if (Input.GetMouseButton(0))
        {
            if (mousePressed == false)
            {
                mousePressed = true;
                rotationTime = 0;
                //startPosition_y = transform.position.y;
            }

           

            if (rotationTime < rotationSpeed)
            {
                newRot.z = Mathf.Lerp(transform.eulerAngles.z, maxAngle + 90, rotationTime / rotationSpeed);
                accel_y = Mathf.Lerp(/*speedPerFrame_x * Mathf.Tan(transform.eulerAngles.z - 90)*/accel_y, speedPerFrame_y, rotationTime / rotationSpeed);
                rotationTime += Time.deltaTime;           
            }
            else
            {
                newRot.z = maxAngle + 90;              
            }
            newPos.y = transform.position.y + accel_y;
        }
        else
        {
            if (mousePressed == true)
            {
                mousePressed = false;
                rotationTime = 0;
               // startPosition_y = transform.position.y;
            }
            

            if (rotationTime < rotationSpeed)
            {
                newRot.z = Mathf.Lerp(transform.eulerAngles.z, -maxAngle + 90, rotationTime / rotationSpeed);
                accel_y =  Mathf.Lerp(/*speedPerFrame_x * Mathf.Tan(transform.eulerAngles.z - 90)*/accel_y, -speedPerFrame_y,  rotationTime / rotationSpeed);
                rotationTime += Time.deltaTime;
            }
            else
            {
                newRot.z = -maxAngle + 90;
               // newPos.y = transform.position.y - speedPerFrame_y;
            }
            newPos.y = transform.position.y + accel_y;
        }

        transform.position = newPos;
        transform.eulerAngles = newRot;
    }

}
