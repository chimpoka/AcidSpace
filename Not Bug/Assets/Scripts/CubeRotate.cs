using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotate : MonoBehaviour {

    public float rotationSpeed = 1.2f;
    public float offsetAngle = 0f;
    public bool xRotation = false;
    public bool yRotation= true;
    public bool zRotation = true;

    private bool[] boolRotation;
    private int[] intRotation;
    private Vector3 angles;

    private void Awake()
    {
        gameObject.isStatic = false;
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = Resources.Load("Materials/mat_RotatedBlocks") as Material;
        meshRenderer.material.mainTextureScale = transform.localScale / 20;
    }

    private void Start()
    {
        if (!xRotation && !yRotation && !zRotation)
        {
            yRotation = true;
            zRotation = true;
        }

        boolRotation = new bool[] { xRotation, yRotation, zRotation };
        intRotation = new int[3];

        for (int i = 0; i < boolRotation.Length; i++)
        {
            if (boolRotation[i] == true)
                intRotation[i] = 1;
            else
                intRotation[i] = 0;
        }

        Vector3 offset = new Vector3(offsetAngle * intRotation[0], offsetAngle * intRotation[1], offsetAngle * intRotation[2]);
        angles = transform.eulerAngles + offset;
    }

    void Update ()
    {
        if (Controller.gameMode == GameMode.Play)
        {
            angles = new Vector3(angles.x + rotationSpeed * intRotation[0], angles.y + rotationSpeed * intRotation[1], angles.z + rotationSpeed * intRotation[2]);
            transform.eulerAngles = angles;
        }
	}
}
