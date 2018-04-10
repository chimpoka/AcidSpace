using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
	void Start ()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach(Renderer r in renderers)
        {
            r.material.mainTextureScale = new Vector2(r.gameObject.transform.localScale.x / 13, r.gameObject.transform.localScale.y / 13);
        }
	}
	
	
}
