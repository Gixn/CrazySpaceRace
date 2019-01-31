using UnityEngine;
using UnityEngine.Networking;

public class CollectibleWall : MonoBehaviour {
        
    void OnTriggerEnter(Collider other) {
        var playerLogic = other.transform.parent.gameObject.GetComponent<PlayerLogic>();

        //collided with player
        if (playerLogic != null /* and player local */) {
            playerLogic.ActionWall();
            Destroy(gameObject);
        }
    }
}
