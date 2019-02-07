using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, ITouchDetectorDelegate
{
    public GameObject player;
    public GameObject wall;
    public GameObject boost;
    public GameObject endGameFront;
    public GameObject endGameBack;
    
    private Map map;
    private PlayerLogic playerLogic;

    private const int spawnPosition = 1000;
    private const int endGameFrontPosition = 2000;
    private const int endGameBackPosition = 100;
    private bool gameOver = false;

    private int speed = 5;

    void Start() {      
        setupControls();
        buildMap();
        player = Instantiate(player);
        endGameFront = Instantiate(endGameFront);
        endGameBack = Instantiate(endGameBack);
        setupGameEndPSs(); 
        
        playerLogic = player.GetComponent<PlayerLogic>();
    }

    private void setupGameEndPSs()
    {
        var endGameBackPS1 = endGameBack.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        endGameBackPS1.transform.localPosition = endGameBackPS1.transform.localPosition + new Vector3(Segment.LaneOffset/endGameBack.transform.localScale.x, 0, 0);
        var endGameBackPS2 = endGameBack.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        endGameBackPS2.transform.localPosition = endGameBackPS2.transform.localPosition + new Vector3(-Segment.LaneOffset/endGameBack.transform.localScale.x, 0, 0);
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
        map = new Map(0,objects);
        map.GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver) return;

        var playerPos = spawnPosition + playerLogic.nodeOffset;
        if (playerPos<=endGameBackPosition)
        {
            playerLogic.nodeOffset = endGameFrontPosition-spawnPosition-100;
        }
        else if (playerPos>=endGameFrontPosition)
        {
            gameOver = true;
        }
        else
        {
            map.Next(speed);

            player.transform.position = map.GetNode(playerPos).transform.position;
            player.transform.rotation = map.GetNode(playerPos).transform.rotation;

            endGameFront.transform.position =  map.GetNode(endGameFrontPosition).transform.position;
            endGameFront.transform.rotation =  map.GetNode(endGameFrontPosition).transform.rotation;
            
            endGameBack.transform.position =  map.GetNode(endGameBackPosition).transform.position;
            endGameBack.transform.rotation =  map.GetNode(endGameBackPosition).transform.rotation;
        }
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