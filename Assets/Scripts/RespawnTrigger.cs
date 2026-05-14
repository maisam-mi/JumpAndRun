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
        UIManager.Instance.ShowGameOver();
        Character character = controller.gameObject.GetComponent<Character>();
        character.InflictDamage(character.GetMaxHealth());
    }
}
