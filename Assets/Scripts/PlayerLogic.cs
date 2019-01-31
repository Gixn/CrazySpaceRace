using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerLogic : NetworkBehaviour{
    public float parentScaledLaneOffset = 0f;
    public float actualLineOffset = 0f;
    public int nodeOffset = 100;
    
    void Start() {
        Camera.main.transform.parent = transform;
        Camera.main.transform.localPosition = new Vector3(0,-10f,-5f);
        Camera.main.transform.localRotation = Quaternion.Euler(-80,0,0);
    }


    public void actionWall()
    {
        nodeOffset -= 100;
    }
    
    public void actionBoost() {
        nodeOffset += 100;
    }
    
    
}
