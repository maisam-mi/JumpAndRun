using UnityEngine;

public class SpikeController : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float damagePerSecound = 25f;

    [Header("Movement")]
    [SerializeField]
    private float spikeSpeed;
    [SerializeField]
    private Vector3 start;
    [SerializeField]
    private Vector3 end;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 90f;

    private Vector3 lastPosition;


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var character = other.GetComponent<Character>();
            character.InflictDamage(this.damagePerSecound * Time.fixedDeltaTime);
        }
    }

    void FixedUpdate()
    {
        lastPosition = transform.position;

        float pingPong = Mathf.PingPong(Time.fixedTime * this.spikeSpeed, 1.0f);

        var newPosition = Vector3.Lerp(this.start, this.end, pingPong);
        this.transform.localPosition = newPosition;

        gameObject.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
