using UnityEngine;

public class HeartController : MonoBehaviour
{
    [SerializeField] private Character character;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 90f;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        character.IncreaseHealth();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        gameObject.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
