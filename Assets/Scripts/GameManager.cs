using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, ITouchDetectorDelegate
{        
    public GameObject player;
    public GameObject wall;
    public GameObject boost;
    public GameObject endGameFront;
    public GameObject endGameBack;
    
    // ui elements
    public Text scoreText;
    public Text text1;
    public Text text2;
    public Button button1;
    public Button button2;
       
    private Map map;
    private PlayerLogic playerLogic;

    private Boolean gameIsActive = false;
    
    private const int spawnPosition = 1000;
    private int endGameFrontPosition = 2000;
    private const int endGameBackPosition = 0;
    private bool gameOver = false;

    private int speed = 5;
    private float speedChangeInterval = 0.03f;

    void Start() {      
        setupControls();
        buildMap();
        player = Instantiate(player);
        endGameFront = Instantiate(endGameFront);
        endGameBack = Instantiate(endGameBack);
        setupGameEndPSs(); 
        
        playerLogic = player.GetComponent<PlayerLogic>();
        StartCoroutine(changeDifficulty());
        
        setGamesStartUI();
    }

    private void Restart()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().name );
    }

    private IEnumerator changeDifficulty()
    {
        while (true)
        {
            endGameFrontPosition-=1;
            yield return new WaitForSeconds(speedChangeInterval);
        }
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
    void FixedUpdate()
    {
        if (!gameIsActive)
        {
            return;
        }
        
        if (gameOver)
        {
            GameOver();
            return;
        };

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
            player.transform.position = map.GetNode(playerPos).transform.position;
            player.transform.rotation = map.GetNode(playerPos).transform.rotation;

            endGameFront.transform.position =  map.GetNode(endGameFrontPosition).transform.position;
            endGameFront.transform.rotation =  map.GetNode(endGameFrontPosition).transform.rotation;
            
            endGameBack.transform.position =  map.GetNode(endGameBackPosition).transform.position;
            endGameBack.transform.rotation =  map.GetNode(endGameBackPosition).transform.rotation;

            map.Next(speed);
        }
        scoreText.text = "Score: " + map.GetScore();
    }


    private void GameOver()
    {
        setGameEndUI();
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
    
    // ui methods

    private void setGamesStartUI(){
        button1.gameObject.SetActive(true);
        button2.gameObject.SetActive(false);
        text1.gameObject.SetActive(true);
        text2.gameObject.SetActive(true);

        button1.GetComponentsInChildren<Text>()[0].text = "Start Game";
        text1.text = "CrazySpaceRace";
        text2.text = "Lets start the ride and don't forget - never touch the Lightning-Storm!";
        
        button1.onClick.RemoveAllListeners();
        button1.onClick.AddListener(ClickedButton1Start);
    }
    
    private void setGameEndUI(){
        button1.gameObject.SetActive(true);
        button2.gameObject.SetActive(true);
        text1.gameObject.SetActive(true);
        text2.gameObject.SetActive(true);

        button1.GetComponentsInChildren<Text>()[0].text = "Restart";
        button2.GetComponentsInChildren<Text>()[0].text = "End Game";
        text1.text = "Game Over!";
        text2.text = "Unbelievable - you reached " + map.GetScore() + " Crazy-Space-Points!";
        
        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        button1.onClick.AddListener(ClickedButton1End);
        button2.onClick.AddListener(ClickedButton2End);

    }

    private void HideGameUI(){
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        text1.gameObject.SetActive(false);
        text2.gameObject.SetActive(false);
    }

    private void ClickedButton1Start(){
        HideGameUI();
        gameIsActive = true;
    }
    
    private void ClickedButton1End(){
        Restart();
        HideGameUI();
        gameIsActive = true;
    }

    private void ClickedButton2End(){
        
    }



}