using UnityEngine;

public class Ball : MonoBehaviour
{
    #region Public Fields

    public float minDirectionSpeed = 0.1f;

    public float startSpeed = 2.0f;
    public float maxSpeed = 10.0f;
    public float collisionVelocityMultiplier = 1.1f;

    #endregion

    #region Private Fields

    private Collider lastHitCollider = null;

    private float hitTime = -1.0f;

    private Rigidbody rb;

    #endregion

    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        
        Vector3 startForce = Random.onUnitSphere;
        startForce.z = 0.0f;

        if (Mathf.Abs(startForce.x) < this.minDirectionSpeed)
        {
            startForce.x += minDirectionSpeed / startForce.x;
        }
        if (Mathf.Abs(startForce.y) < this.minDirectionSpeed)
        {
            startForce.y += minDirectionSpeed / startForce.y;
        }
        startForce = startForce.normalized;
        startForce *= this.startSpeed;

        this.rb.AddForce(startForce, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider != this.lastHitCollider && Time.time > this.hitTime)
        {

            var velocity = this.rb.linearVelocity;
            velocity *= this.collisionVelocityMultiplier;
            var contact = collision.contacts[0];

            if(Vector3.Dot(contact.normal, velocity) < 0.0f)
            {
                velocity = Vector3.Reflect(velocity, contact.normal);
            }

            if(velocity.magnitude > this.maxSpeed)
            {
                velocity *= (this.maxSpeed / velocity.magnitude);
            }

            float velocityXAbs = Mathf.Abs(velocity.x);
            float velocityYAbs = Mathf.Abs(velocity.y);

            if(velocityXAbs < this.minDirectionSpeed && velocityXAbs != 0.0f)
            {
                velocity.x *= (this.minDirectionSpeed / velocityXAbs);
            } else if(Mathf.Approximately(velocityXAbs, 0.0f))
            {
                velocity.x = Random.Range(-this.minDirectionSpeed, this.minDirectionSpeed);
            }

            if(velocityYAbs < this.minDirectionSpeed && velocityYAbs != 0.0f)
            {
                velocity.y *= (this.minDirectionSpeed / velocityYAbs);
            } else if(Mathf.Approximately(velocityYAbs, 0.0f))
            {
                velocity.y = Random.Range(-this.minDirectionSpeed, this.minDirectionSpeed);
            }

            this.rb.linearVelocity = velocity;
            this.lastHitCollider = collision.collider;
            this.hitTime = Time.time;
        }

    }

}
