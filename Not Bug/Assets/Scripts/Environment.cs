using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour {

    public Color[] wallsColors;
    public Color[] spaceColors;
    public float changeTime = 10.0f;
    public int changeEnvironmentPoint = 300;

    private int state = -1;
    private List<MeshRenderer> meshes;
    private float time = 0;
    private int statesNumber = 0;
    private bool startChanging = false;
    private int first = 0;
    private int second = 0;
    

	void Start ()
    {
        meshes = new List<MeshRenderer>();

        MeshRenderer[] obstacles = GameObject.Find("Obstacles").GetComponentsInChildren<MeshRenderer>();
        MeshRenderer[] walls = GameObject.Find("Walls").GetComponentsInChildren<MeshRenderer>();
        MeshRenderer space = GameObject.Find("Space (1)").GetComponent<MeshRenderer>();

        meshes.Add(space);
        foreach (MeshRenderer o in obstacles)
            meshes.Add(o);
        foreach (MeshRenderer w in walls)
            meshes.Add(w);

        statesNumber = spaceColors.Length;
    }
	

	void Update ()
    {
        if (state >= 0)
        {
            if (startChanging == false)
            {
                startChanging = true;
                while (first == second)
                    second = Random.Range(0, statesNumber - 1);
            }

            foreach (MeshRenderer mesh in meshes)
            {
                // Цвет объекта Space
                meshes[0].material.SetColor("_EmissionColor", Color.Lerp(spaceColors[first], spaceColors[second], time / changeTime));
                //meshes[0].material.color = Color.Lerp(spaceColors[state], spaceColors[state + 1], time / changeTime);

                // Цвет вращающихся препятствий
                if (mesh.material.name == "mat_RotatedBlocks (Instance)")
                    mesh.material.color = Color.Lerp(wallsColors[first], wallsColors[second], time / changeTime);

                // Цвет неподвижных препятствий и стен
                else if (mesh.material.name == "mat_WorldAlignTexture (Instance)")
                {
                    mesh.material.SetColor("_ColorX", Color.Lerp(wallsColors[first], wallsColors[second], time / changeTime));
                    mesh.material.SetColor("_ColorY", Color.Lerp(wallsColors[first], wallsColors[second], time / changeTime));
                   // mesh.material.SetColor("_ColorZ", Color.Lerp(wallsColors[state], wallsColors[state + 1], time / changeTime));
                }
            }
            time += Time.deltaTime;

            if (time > changeTime)
            {
                time = 0;
                state = -1;
                startChanging = false;
                first = second;
            }
        }
    }

    public void ChangeEnvironment()
    {
        state = 1;
    }
}
