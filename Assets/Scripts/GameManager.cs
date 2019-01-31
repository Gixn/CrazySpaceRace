using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, ITouchDetectorDelegate
{
    public GameObject player;
    private GameObject vehicle;
    private TextMesh boostTime;
    private PlayerLogic playerLogic;
    
    public GameObject wall;
    public GameObject boost;

    public GameObject blackHole;
    
    private Map map;
    
    private float currCountdownValue;
    private float cooldownValue = 7;
    private float currBoostDurationValue;
    private float boostDurationValue = 3;

    private bool boostCooldown = false;

    
    void Start() {      
        var touchDetector = GameObject.Find("Main Camera").GetComponent<TouchDetector>();
        touchDetector.TouchDelegate = this;
        
        var objects = new List<GameObject>();
        objects.Add(wall);
        objects.Add(boost);
        map = new Map(-1,objects);
        
        player = Instantiate(player);
        vehicle = player.transform.GetChild(0).gameObject;
        boostTime = player.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
        boostTime.text = cooldownValue.ToString();
        playerLogic = player.GetComponent<PlayerLogic>();

        blackHole = Instantiate(blackHole);
        
        playerLogic.parentScaledLaneOffset = Segment.LaneOffset / player.transform.localScale.x;
        
        map.GenerateMap();
    }

    // Update is called once per frame
    void Update() {
        player.transform.position = map.GetNode(playerLogic.nodeOffset).transform.position;
        player.transform.rotation = map.GetNode(playerLogic.nodeOffset).transform.rotation;
        
        blackHole.transform.position =  map.GetNode(playerLogic.nodeOffset+200).transform.position;
        blackHole.transform.rotation =  map.GetNode(playerLogic.nodeOffset+200).transform.rotation;

        map.Next(5);
    }

    // touch callback functions
    public void OnSwipeUp(){
        Debug.Log("Swipe Up");
        if (!boostCooldown) {
            StartCoroutine(StartBoostCountdown(boostDurationValue));
            StartCoroutine(StartCooldownCountdown(cooldownValue,boostTime));
        }
       
    }

    public void OnTouchLeftHalf(){
        Debug.Log("Touch Left Side");
        
        if (playerLogic.actualLineOffset > -playerLogic.parentScaledLaneOffset) {
            playerLogic.actualLineOffset -= playerLogic.parentScaledLaneOffset;
            
            vehicle.transform.localPosition = new Vector3(playerLogic.actualLineOffset,0,0);
        }
    }

    public void OnTouchRightHalf(){
        Debug.Log("Touch Right Side");
        
        if (playerLogic.actualLineOffset < playerLogic.parentScaledLaneOffset) {
            playerLogic.actualLineOffset += playerLogic.parentScaledLaneOffset;
            
            vehicle.transform.localPosition = new Vector3(playerLogic.actualLineOffset,0,0);
        }
    }
    
    
    // boost timer
    private IEnumerator StartBoostCountdown(float boostDurationValue)
    {     
        playerLogic.boostActive = true;
        while (currBoostDurationValue > 0)
        {
            //ToDo: fly faster! - workt nicht!
            //playerLogic.nodeOffset += 50;

            yield return new WaitForSeconds(1.0f);
            currBoostDurationValue--;
        }
        playerLogic.boostActive = false;
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