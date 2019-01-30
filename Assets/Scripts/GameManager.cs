using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, ITouchDetectorDelegate
{
    public GameObject player;
    private GameObject vehicle;
    private PlayerLogic2 playerLogic;
    
    public GameObject wall;
    public GameObject boost;
   
    private Map map;
    
    void Start() {      
        var touchDetector = GameObject.Find ("Main Camera").GetComponent<TouchDetector>();
        touchDetector.TouchDelegate = this;
        
        player = Instantiate(player);
        var objects = new List<GameObject>();
        objects.Add(wall);
        objects.Add(boost);
        
        map = new Map(-1,objects);
        
        vehicle = player.transform.GetChild(0).gameObject;
        playerLogic = player.GetComponent<PlayerLogic2>();
        
        playerLogic.parentScaledLaneOffset = Segment.LaneOffset / player.transform.localScale.x;
        
        map.GenerateMap();
    }

    // Update is called once per frame
    void Update() {
        player.transform.position = map.GetNode(playerLogic.nodeOffset).transform.position;
        player.transform.rotation = map.GetNode(playerLogic.nodeOffset).transform.rotation;
        
        map.Next(5);
    }

    // touch callback functions
    public void OnSwipeUp(){
        Debug.Log("Swipe Up");
        
        playerLogic.nodeOffset += 100;
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

}