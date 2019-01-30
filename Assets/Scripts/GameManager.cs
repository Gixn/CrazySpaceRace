using UnityEngine;

public class GameManager : MonoBehaviour, ITouchDetectorDelegate
{
    public GameObject player;
    private GameObject vehicle;
    
    private float parentScaledLaneOffset = 0f;    
    private float actualLineOffset = 0f;

    private Map map=new Map(0);
    void Start()
    {      
        var touchDetector = GameObject.Find ("Main Camera").GetComponent<TouchDetector>();
        touchDetector.TouchDelegate = this;
        
        player = Instantiate(player);
        vehicle = player.transform.GetChild(0).gameObject;

        parentScaledLaneOffset = Map.LaneOffset / player.transform.localScale.x;
        
        map.GenerateMap();
        map.PlaceRandomObjects(GameObject.CreatePrimitive(PrimitiveType.Cube),300);
     }

    // Update is called once per frame
    void Update()
    {
        player.transform.position = map.getNode(100).transform.position;
        player.transform.rotation = map.getNode(100).transform.rotation;
        
        map.next(5);
    }

    // touch callback functions
    public void OnSwipeUp(){
        Debug.Log("Swipe Up");
    }

    public void OnTouchLeftHalf(){
        Debug.Log("Touch Left Side");
        
        if (actualLineOffset > -parentScaledLaneOffset) {
            actualLineOffset -= parentScaledLaneOffset;

            vehicle.transform.localPosition = new Vector3(actualLineOffset,0,0);
        }
    }

    public void OnTouchRightHalf(){
        Debug.Log("Touch Right Side");
        if (actualLineOffset < parentScaledLaneOffset) {
            actualLineOffset += parentScaledLaneOffset;
            
            vehicle.transform.localPosition = new Vector3(actualLineOffset,0,0);
        }    
    }
}