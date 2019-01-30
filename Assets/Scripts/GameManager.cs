using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;

    private Map map=new Map();
    void Start()
    {
        player = Instantiate(player);
        map.Generate(-1);
    }

    // Update is called once per frame
    void Update()
    {
        player.transform.position = map.getNode(100).transform.position;
        player.transform.rotation = map.getNode(100).transform.rotation;

        map.next(5);
    }
}