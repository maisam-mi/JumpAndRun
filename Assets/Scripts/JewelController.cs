using UnityEngine;

public class JewelController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f;

    
    void Update()
    {
        gameObject.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        GameManager.Instance.SetIsWon(true);
        UIManager.Instance.ShowVictory();

        this.gameObject.SetActive(false);
    }
}
