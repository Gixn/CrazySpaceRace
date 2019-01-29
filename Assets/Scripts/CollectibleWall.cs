using UnityEngine;
using UnityEngine.Networking;

public class CollectibleWall : MonoBehaviour {
        
    void OnTriggerEnter(Collider other) {
        //only apply collision logic on server, sync events to clients
        if (!NetworkServer.active) {
            return;
        }

        var player = other.GetComponent<PlayerLogic>();

        //collided with player
        if (player != null) {
            player.actionWall();
            Destroy(gameObject);
        }
    }
}
