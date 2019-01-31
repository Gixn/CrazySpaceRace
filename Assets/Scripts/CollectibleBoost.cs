using UnityEngine;
using UnityEngine.Networking;


public class CollectibleBoost : MonoBehaviour {
        
    void OnTriggerEnter(Collider other) {
        var playerLogic = other.transform.parent.gameObject.GetComponent<PlayerLogic>();

        //collided with player
        if (playerLogic != null) {
            playerLogic.actionBoost();
            Destroy(gameObject);
        }
    }
}
