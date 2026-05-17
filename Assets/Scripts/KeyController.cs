using UnityEngine;

public class KeyController : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 90f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        GameManager.Instance.SetDoorState(true);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        gameObject.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
