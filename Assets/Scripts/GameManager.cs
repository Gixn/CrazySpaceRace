﻿using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, ITouchDetectorDelegate
{
    public GameObject player;
    public GameObject wall;
    public GameObject boost;
    public GameObject blackHole;
    
    private Map map;
    private PlayerLogic playerLogic;

    private const int spawnPosition = 100;
    private const int blackHolePosition = 500;
    private int speed = 5;

    void Start() {      
        setupControls();
        buildMap();
        player = Instantiate(player);
        blackHole = Instantiate(blackHole);
        playerLogic = player.GetComponent<PlayerLogic>();
    }

    private void setupControls()
    {
        var touchDetector = GameObject.Find("Main Camera").GetComponent<TouchDetector>();
        touchDetector.TouchDelegate = this;
    }

    private void buildMap()
    {
        var objects = new List<GameObject>();
        objects.Add(wall);
        objects.Add(boost);
        map = new Map(-1,objects);
        map.GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        player.transform.position = map.GetNode(spawnPosition+playerLogic.nodeOffset).transform.position;
        player.transform.rotation = map.GetNode(spawnPosition+playerLogic.nodeOffset).transform.rotation;
        
        blackHole.transform.position =  map.GetNode(blackHolePosition).transform.position;
        blackHole.transform.rotation =  map.GetNode(blackHolePosition).transform.rotation;

        map.Next(speed);
    }

    // touch callback functions
    public void OnSwipeUp(){
        Debug.Log("Swipe Up");
        playerLogic.Boost();
    }

    public void OnTouchLeftHalf(){
        Debug.Log("Touch Left Side");
        playerLogic.MoveLeft();
    }

    public void OnTouchRightHalf(){
        Debug.Log("Touch Right Side");
        playerLogic.MoveRight();
    }
    

    
   

}