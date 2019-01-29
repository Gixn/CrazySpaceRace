using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Net.Sockets;
using System.Net;
using System.Text;

public class GameLogic : MonoBehaviour {
    public GameObject collectibleWallPrefab;
    public GameObject collectibleBoostPrefab;
    public GameObject gameWorld;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        //logic here only performed on server
        if (!NetworkServer.active) {
            return;
        }

        var collectibleWall = FindObjectOfType<CollectibleWall>();

        //spawn collectible at random X/Z pos if none exists
        if (collectibleWall == null) {
            float maxPos = 7.5f;
            float yPos = -5;
            collectibleWall = (Instantiate(
                collectibleWallPrefab, //template
                new Vector3( //position
                    Random.Range(-maxPos, maxPos), yPos,
                    Random.Range(-maxPos, maxPos)),
                Quaternion.identity, //rotation
                gameWorld.transform //parent
            ) as GameObject).GetComponent<CollectibleWall>();

            NetworkServer.Spawn(collectibleWall.gameObject);
        }
        
        var collectibleBoost = FindObjectOfType<CollectibleBoost>();
        if (collectibleBoost == null) {
            float maxPos = 7.5f;
            float yPos = -5;
            collectibleBoost = (Instantiate(
                collectibleBoostPrefab, //template
                new Vector3( //position
                    Random.Range(-maxPos, maxPos), yPos,
                    Random.Range(-maxPos, maxPos)),
                Quaternion.identity, //rotation
                gameWorld.transform //parent
            ) as GameObject).GetComponent<CollectibleBoost>();

            NetworkServer.Spawn(collectibleBoost.gameObject);
        }
    }
}