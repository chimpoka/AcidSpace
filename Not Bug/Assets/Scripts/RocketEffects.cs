using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketEffects : MonoBehaviour
{
    [Range(0, 1)]
    public float slowTimeScale = 0.5f;
    public float slowDuration = 70f;
    public float decelerationTime = 1f;
    public float invulnerabilityDuration = 40f; 
    public float strayTime = 1.0f;
    public GameObject shield;

    [HideInInspector]
    public bool isRocketSlow = false;
    [HideInInspector]
    public float slowScore;
    [HideInInspector]
    public bool isRocketInvulnerable = false;
    [HideInInspector]
    public float invulnerabilityScore;
    [HideInInspector]
    public bool isStray = false;
    [HideInInspector]
    public float strayDeltaTime = 0;
    [HideInInspector]
    public float decelerationDeltaTime = 0;

    private float loop = 0f;
    private float strayScore;
    private bool shieldActivation;



    private void Update()
    {
        if (isStray)
            Stray();

        if (isRocketSlow)
            DoDeceleration();

        if (isRocketInvulnerable)
            DoInvulnerability();
    }

    private void Stray()
    {
        if (strayDeltaTime >= 0 + loop && strayDeltaTime < 0.03 + loop)
            shield.SetActive(false);
        else if (strayDeltaTime >= 0.05 + loop && strayDeltaTime < 0.08 + loop)
            shield.SetActive(true);
        else if (strayDeltaTime >= 0.15 + loop && strayDeltaTime < 0.18 + loop)
            shield.SetActive(false);
        else if (strayDeltaTime >= 0.18 && strayDeltaTime < 0.21 + loop)
            shield.SetActive(true);
        else if (strayDeltaTime >= 0.30 && strayDeltaTime < 0.33 + loop)
            shield.SetActive(false);
        else if (strayDeltaTime >= 0.35 + loop && strayDeltaTime < 0.38 + loop)
            shield.SetActive(true);
        else if (strayDeltaTime >= 0.40 + loop && strayDeltaTime < 0.43 + loop)       
            shield.SetActive(false);       
        else if (strayDeltaTime >= 0.45 + loop && strayDeltaTime < 0.48 + loop)
        {
            shield.SetActive(true);
            loop += 0.5f;
        }
            
        strayDeltaTime += Time.deltaTime;

        if (strayDeltaTime >= strayTime)
        {
            if (shieldActivation)
                shield.SetActive(true);
            else
                shield.SetActive(false);

            strayScore = Controller.score - invulnerabilityScore;
            strayDeltaTime = 0;
            loop = 0;
            isStray = false;
        }
    }


    private void DoDeceleration()
    {
        if (Controller.score >= slowScore + slowDuration)
        {
            Time.timeScale = Mathf.Lerp(1.0f, slowTimeScale, decelerationDeltaTime / decelerationTime);
            decelerationDeltaTime -= Time.deltaTime;

            if (decelerationDeltaTime <= 0)
                isRocketSlow = false;
        }
        else
        {
            if (decelerationDeltaTime <= decelerationTime)
            {
                Time.timeScale = Mathf.Lerp(1.0f, slowTimeScale, decelerationDeltaTime / decelerationTime);
                decelerationDeltaTime += Time.deltaTime;
            }
        }
    }

    private void DoInvulnerability()
    {
        if ((Controller.score == invulnerabilityScore) && isStray == false)
        {
            isStray = true;
            shieldActivation = true;
            Debug.Log(shieldActivation);
        }
        else if ((Controller.score == invulnerabilityScore + invulnerabilityDuration - strayScore) && isStray == false)
        {
            isStray = true;
            shieldActivation = false;
        }

        if (Controller.score == invulnerabilityScore + invulnerabilityDuration)
            isRocketInvulnerable = false;
    }
}
