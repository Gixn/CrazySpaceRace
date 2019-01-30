using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject wall;
    public GameObject boost;

    private Map map;
    void Start()
    {      
        player = Instantiate(player);
        var objects = new List<GameObject>();
        objects.Add(wall);
        objects.Add(boost);

        map=new Map(-1,objects);
        map.GenerateMap();

    }

    // Update is called once per frame
    void Update()
    {
        player.transform.position = map.GetNode(100).transform.position;
        player.transform.rotation = map.GetNode(100).transform.rotation;

        map.Next(5);
    }
}