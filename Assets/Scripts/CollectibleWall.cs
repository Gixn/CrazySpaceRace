using UnityEngine;
using UnityEngine.Networking;

public class CollectibleWall : MonoBehaviour
{
    private ParticleSystem WallAnimation;
    
    private void OnEnable()
    {
        WallAnimation = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        /*WallAnimation.Play();
        Invoke("PauseParticleSystem", 1.9f);*/
    }

    void PauseParticleSystem()
    {
        WallAnimation.Pause();
    }

    void OnTriggerEnter(Collider other) {
        var playerLogic = other.transform.parent.gameObject.GetComponent<PlayerLogic>();

        //collided with player
        if (playerLogic != null /* and player local */) {
            playerLogic.ActionWall();
            WallAnimation.Play();
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject,3);
        }
    }
}
