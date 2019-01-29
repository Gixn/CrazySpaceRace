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

        //TODO: fix speed
        //nodes are unevenly distributed which results in different speeds
        //solution could be to use the angle*multiplier for the position calculation
        //for the straight use the circumference depending on the radius as reference
        //and to skip a increasing/decreasing amount of nodes

        player.transform.position = map.getNode().transform.position;
        player.transform.rotation = map.getNode().transform.rotation;
//
//        float step = 0.1f * Time.deltaTime;
//        player.transform.position=Vector3.MoveTowards(map.getNode().transform.position,player.transform.position, step);
//        player.transform.rotation=Quaternion.RotateTowards(map.getNode().transform.rotation,player.transform.rotation, step);

        map.next(3);
        map.Update();
    }
}