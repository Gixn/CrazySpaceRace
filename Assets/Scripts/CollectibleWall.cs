using UnityEngine;
using UnityEngine.Networking;

public class CollectibleWall : MonoBehaviour {
        
    void OnTriggerEnter(Collider other) {
        var playerLogic = other.transform.parent.gameObject.GetComponent<PlayerLogic2>();

        //collided with player
        if (playerLogic != null /* and player local */) {
            playerLogic.actionWall();
            Destroy(gameObject);
        }
    }
}
