using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [SerializeField] private List<Vector3> positions;
    [SerializeField] private float speed;
    [SerializeField] private float waitedTime;
    [SerializeField] private AudioSource squashSound;

    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        this.animator = this.GetComponent<Animator>();
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

        squashSequence.Append(transform.DOScaleX(1.3f, 0.25f));
        squashSequence.Join(transform.DOScaleZ(1.3f, 0.25f));
        squashSequence.Join(transform.DOScaleY(0.4f, 0.25f));
        squashSequence.AppendInterval(0.1f);
        squashSequence.Append(transform.DOScale(Vector3.zero, 0.2f));
        squashSequence.OnComplete(() => Destroy(gameObject));
    }
}