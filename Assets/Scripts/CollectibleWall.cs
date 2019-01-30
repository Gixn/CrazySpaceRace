using UnityEngine;
using UnityEngine.Networking;

public class CollectibleWall : MonoBehaviour {
        
    void OnTriggerEnter(Collider other) {
        var player = other.GetComponent<PlayerLogic2>();

        //collided with player
        if (player != null /* and player local */) {
            player.actionWall();
            Destroy(gameObject);
        }
    }
}
