using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerLogic2 : NetworkBehaviour {
    void Start() {
        Camera.main.transform.parent = transform;
        Camera.main.transform.localPosition = new Vector3(0,-10f,-5f);
        Camera.main.transform.localRotation = Quaternion.Euler(-80,0,0);
    }


}
