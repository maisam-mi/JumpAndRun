using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.TryGetComponent<CharacterController>(out var controller))
        {
            Respawn(controller);
        }
    }

    private void Respawn(CharacterController controller)
    {
        controller.enabled = false;
        controller.transform.position = respawnPoint.position;
        controller.enabled = true;
    }
}
