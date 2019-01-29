using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update


    private MapGenerator mapGenerator=new MapGenerator();
    void Start()
    {

        mapGenerator.GenerateSegments();
        //segment.ForEach(s=>Instantiate(s));

        

        Debug.Log("start");

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("update");
    }
}
