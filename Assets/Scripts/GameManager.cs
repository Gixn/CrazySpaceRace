using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;

    private Map map=new Map();
    void Start()
    {
        player = Instantiate(player);
        map.Generate(0);
    }

    // Update is called once per frame
    void Update()
    {
        player.transform.position = map.getNode().transform.position;
        player.transform.rotation = map.getNode().transform.rotation;

        map.Update();
    }
}