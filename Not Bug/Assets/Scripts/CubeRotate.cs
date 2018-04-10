using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotate : MonoBehaviour {

    public float rotationSpeed = 1.2f;

    private void Awake()
    {
        gameObject.isStatic = false;
    }

    void Update ()
    {
        if (Controller.mode == GameMode.Play)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + rotationSpeed, transform.eulerAngles.z + rotationSpeed);
	}
}
