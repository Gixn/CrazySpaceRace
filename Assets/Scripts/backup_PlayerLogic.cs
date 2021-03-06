﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class backup_PlayerLogic : NetworkBehaviour {

    public float moveSpeed = 10, rotateSpeed = 4;

    //synchronize points from server to all clients
    [SyncVar]
    private int playerOffset;

    [SyncVar]
    private Color color;

    //public accessor
    public int PlayerOffset
    {
        get { return playerOffset; }
        set { playerOffset = value; }
    }

	// Use this for initialization
	void Start ()
    {
        var networkId = GetComponent<NetworkIdentity>();

        if (NetworkServer.active)
        {
            //server "chooses" color for players (SyncVars sync from server to client)
            color = Random.ColorHSV(
                0, 1,
                0.5f, 1, 
                0.5f, 1); //generates colors that are neither too dark nor too bright
        }
    }
	
	// Update is called once per frame
	void Update () {
        GetComponent<Renderer>().material.color = this.color;

        var isLocal = isLocalPlayer;
        
        //only control local player object
        if (isLocal) {
            control();

            //attach main camera to local player and a few units back (3rd person view)
            Camera.main.transform.parent = transform;
            Camera.main.transform.localPosition = Vector3.forward * -2;
            Camera.main.transform.localRotation = Quaternion.identity;
        }
        
        
    }

    private void OnDestroy() {
        //make camera global again instead of following player

        var camera = GetComponentInChildren<Camera>();
        if (camera != null)
        {
            camera.transform.parent = null;
            camera.enabled = true;
            camera.transform.position = new Vector3(0, 1, -10);
            camera.transform.rotation = Quaternion.Euler(45, 0, 0);
        }
    }

    private void control()
    {
        var body = GetComponent<Rigidbody>();

        body.AddForce(transform.forward * moveSpeed * Input.GetAxis("Vertical"));

        body.AddTorque(Vector3.up * rotateSpeed * Input.GetAxis("Horizontal"));

        var networkTransform = GetComponent<NetworkTransform>().transform;

        //reset player if fallen down
        if (networkTransform.position.y < -10)
        {
            networkTransform.position = Vector3.zero;
            networkTransform.rotation = Quaternion.identity;

            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;

            CmdResetPlayerOffset();
        }
    }

    public void actionWall() {
        playerOffset -= 2;
        var networkTransform = GetComponent<NetworkTransform>().transform;
        var vec = networkTransform.position;
        vec.z += 2;
        networkTransform.position = vec;
    }
    
    public void actionBoost() {
        playerOffset += 2;
        var networkTransform = GetComponent<NetworkTransform>().transform;
        var vec = networkTransform.position;
        vec.z += 2;
        networkTransform.position = vec;
    }

    [Command]
    public void CmdResetPlayerOffset()
    {
        if(NetworkServer.active)
        {
            playerOffset = 0;
        }
    }
}
