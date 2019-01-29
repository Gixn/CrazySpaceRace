using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;

    private MapGenerator mapGenerator;
    private int index = 0;
    void Start()
    {
        player = Instantiate(player);
        mapGenerator = new MapGenerator(100);
    }

    // Update is called once per frame
    void Update()
    {
        if (index<mapGenerator.Nodes.Count)
        {
            player.transform.position = mapGenerator.Nodes[index].transform.position;
            player.transform.rotation = mapGenerator.Nodes[index].transform.rotation;
        
            index += 1;
        }
    }
}
