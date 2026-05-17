using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float damagePerSecound = 25f;

    [Header("Movement")]
    [SerializeField] private List<Vector3> positions;
    [SerializeField] private float speed;
    [SerializeField] private float waitedTime;

    [Header("Sound")]
    [SerializeField] private AudioSource squashSound;

    private Animator animator;
    private bool isDead = false;

    void OnEnable()
    {
        this.animator = this.GetComponent<Animator>();

        // Reset death state
        isDead = false;

        // Reset scale (in case it died mid-animation)
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        // Restart movement coroutine
        StartCoroutine(Move());
    }

    void SetAnimationState(bool isWalking)
    {
        this.animator.SetBool("IsWalking", isWalking);
    }

    void FaceDirection(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - this.transform.position);
        direction.y = 0.0f;
        if (direction.sqrMagnitude > 0.0f)
        {
            this.transform.forward = direction.normalized;
        }
    }

    IEnumerator Move()
    {
        while (true)
        {
            for (int i = 0; i < this.positions.Count; i++)
            {
                Vector3 targetPosition = this.positions[i];

                // Face the target before moving
                FaceDirection(targetPosition);
                SetAnimationState(true);

                // Move towards target until close enough
                while (Vector3.Distance(this.transform.position, targetPosition) > 0.05f)
                {
                    this.transform.position = Vector3.MoveTowards(
                        this.transform.position,
                        targetPosition,
                        this.speed * Time.deltaTime
                    );
                    yield return null; // Wait one frame
                }

                // Snap to exact position
                this.transform.position = targetPosition;

                // Wait at position
                SetAnimationState(false);
                yield return new WaitForSeconds(this.waitedTime);
            }
        }
    }

    public void GetHit()
    {
        if (!isDead)
        {
            isDead = true;
            squashSound.Play();
            StompedDisappear();
        }
    }

    void StompedDisappear()
    {
        GetComponent<Collider>().enabled = false;
        transform.DOKill();

        Sequence squashSequence = DOTween.Sequence();

        squashSequence.Append(transform.DOScale(new Vector3(1.3f, 0.4f, 1.3f), 0.25f));
        squashSequence.AppendInterval(0.1f);
        squashSequence.Append(transform.DOScale(Vector3.zero, 0.2f));
        squashSequence.OnComplete(() => gameObject.SetActive(false));
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        if(collision.gameObject.TryGetComponent<Character>(out Character character))
            character.InflictDamage(this.damagePerSecound * Time.fixedDeltaTime);
    }
}