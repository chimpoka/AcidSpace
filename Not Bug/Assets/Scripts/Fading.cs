using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fading : MonoBehaviour {

    private float fadeTime;
    private float alpha = 1;
    private bool doFading = false;

    private void Start()
    {
        //alpha = GetComponent<Image>().color.a;
    }

    private void Update()
    {
        if (doFading == true)
        {
            alpha += Time.deltaTime / fadeTime;
        }
        else
        {
            alpha -= Time.deltaTime / fadeTime;
        }

        alpha = Mathf.Clamp01(alpha);
        GetComponent<Image>().color = new Color(0, 0, 0, alpha);
    }

    public void FadeIn(float time)
    {
        fadeTime = time;
        doFading = false;
    }

    public void FadeOut(float time)
    {
        fadeTime = time;
        doFading = true;
    }
}
