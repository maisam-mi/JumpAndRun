using UnityEngine;
using UnityEngine.Events;

public class CoinController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private AudioSource coinCollectSound;

    public UnityEvent coinCollectEvent;


    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            return;
        }

        coinCollectEvent.Invoke();
        coinCollectSound.Play();
        Destroy(gameObject);
    }
}
