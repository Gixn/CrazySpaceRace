using UnityEngine;
using UnityEngine.Networking;

public class CollectibleWall : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        var playerLogic = other.transform.parent.gameObject.GetComponent<PlayerLogic>();

        //collided with player
        if (playerLogic != null /* and player local */) {
            playerLogic.ActionWall();
            var ps = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
            ps.Play();
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject,3);
        }
    }
}
