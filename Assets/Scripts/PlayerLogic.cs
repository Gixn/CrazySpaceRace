using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerLogic : NetworkBehaviour{
    public float parentScaledLaneOffset = 0f;
    public float actualLineOffset = 0f;
    public int nodeOffset = 100;
    

    public GameObject vehicle;
    public ParticleSystem particleSystem;

    private bool boostCooldown = false;
    private bool boostActive = false;

    private float currCountdownValue;
    private float cooldownValue = 7;
    private float currBoostDurationValue=3;
    private float boostDurationValue = 3;
    private float boostPSmin = 0;
    private float boostPSnormal = 2;
    private float boostPSmax = 20;

    private TextMesh boostTime;

    private const int maxWallSpeed = 20;
    private const int maxBoostSpeed = 20;
    private const float boostMultiplicator = 2f;

    void Start() {
        Camera.main.transform.parent = transform;
        Camera.main.transform.localPosition = new Vector3(0,-10f,-5f);
        Camera.main.transform.localRotation = Quaternion.Euler(-80,0,0);

        boostTime = GetComponentInChildren(typeof(TextMesh)) as TextMesh;
        boostTime.text = cooldownValue.ToString();

        parentScaledLaneOffset = Segment.LaneOffset / transform.localScale.x;
    }

    void Update(){
    
    }

    public void ActionWall()
    {
        StartCoroutine(doWallAction());
    }

    private IEnumerator doWallAction()
    {
        int counter = maxWallSpeed;
        if (boostActive) {
            counter = (int)(maxWallSpeed * boostMultiplicator);
        }
        
        var emission = particleSystem.emission;
        emission.rateOverTime = boostPSmin;
        
        while (counter>0)
        {
            nodeOffset -= counter;
            counter -= 1;
            yield return new WaitForSeconds(0.02f);
        }
        emission.rateOverTime = boostPSnormal;
    }

    public void ActionBoost()
    {
        StartCoroutine(doBoostAction());
    }

    private IEnumerator doBoostAction() {
        int counter = maxBoostSpeed;

        var emission = particleSystem.emission;
        emission.rateOverTime = boostPSmax;
        
        while (counter>0) {
            nodeOffset += counter;
            counter -= 1;
            yield return new WaitForSeconds(0.02f);
        }
        
        emission.rateOverTime = boostPSnormal;
    }

    public void MoveLeft()
    {
        if (actualLineOffset > -parentScaledLaneOffset) {
            actualLineOffset -= parentScaledLaneOffset;

            vehicle.transform.localPosition = new Vector3(actualLineOffset,0,0);
        }
    }

    public void MoveRight()
    {
        if (actualLineOffset < parentScaledLaneOffset) {
            actualLineOffset += parentScaledLaneOffset;

            vehicle.transform.localPosition = new Vector3(actualLineOffset,0,0);
        }
    }

    public void Boost() {
        if (!boostCooldown)
        {
            StartCoroutine(doBoostAction());
            StartCoroutine(StartBoostCountdown(boostDurationValue));
            StartCoroutine(StartCooldownCountdown(cooldownValue,boostTime));
        }
    }


    // boost timer
    private IEnumerator StartBoostCountdown(float boostDurationValue)
    {
        boostActive = true;
        while (currBoostDurationValue > 0)
        {
            yield return new WaitForSeconds(1.0f);
            currBoostDurationValue--;
        }

        currBoostDurationValue = boostDurationValue;
        boostActive = false;
    }

    private IEnumerator StartCooldownCountdown(float cooldownValue, TextMesh textMash)
    {
        currCountdownValue = cooldownValue;
        boostCooldown = true;
        while (currCountdownValue > 0)
        {
            Debug.Log("Countdown: " + currCountdownValue);
            textMash.text = currCountdownValue.ToString();

            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
        textMash.text = "ready!";
        boostCooldown = false;
    }
}