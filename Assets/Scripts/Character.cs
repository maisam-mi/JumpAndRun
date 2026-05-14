using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.EventTrigger;

public class Character : MonoBehaviour
{

    [SerializeField] private float characterSpeed;
    [SerializeField] private float dampening;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private AudioSource JumpAudioSource;
    [SerializeField] private AudioSource footstepAudioSource;

    [SerializeField] private ParticleSystem dustParticles;

    private float currentHealth;
    private bool isJumping = false;
    private float jumpCooldownTimer;
    private CharacterController controller;
    private InputAction moveAction;
    private InputAction jumpAction;
    private Vector3 characterMovement;
    private Vector3 jumpVelocity;
    private Vector3 characterGravity;
    private Vector3 platformVelocity;
    private bool isDeath = false;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        this.animator = this.GetComponent<Animator>();
        this.currentHealth = this.maxHealth;
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        jumpCooldownTimer = 0.0f;
        footstepAudioSource.Play();
    }

    //void SetAnimationState()
    //{
    //    this.animator.SetBool("IsJumping", this.isJumping);
    //}

    void SetAnimationState(Vector2 inputMovement)
    {
        this.animator.SetBool("IsJumping", this.isJumping);
        this.animator.SetBool("IsRunning", inputMovement != Vector2.zero);
        this.animator.SetFloat("MovementForward", inputMovement.magnitude);
        this.animator.SetBool("IsDeath", this.isDeath);
    }

    public float GetCurrentHealth() => this.currentHealth;
    public float GetMaxHealth() => this.maxHealth;

    public void InflictDamage(float amount)
    {
        this.currentHealth -= amount;
        this.currentHealth = Mathf.Clamp(this.currentHealth, 0.0f, this.maxHealth);
    }

    void HandleJumping()
    {
        if (this.controller.isGrounded && this.isJumping && this.jumpCooldownTimer <= 0.0f)
        {
            this.jumpVelocity = Vector3.zero;
            this.isJumping = false;
        }
        if (this.controller.isGrounded && !this.isJumping && this.jumpAction.WasPressedThisFrame())
        {
            this.characterGravity = Vector3.zero;
            this.jumpVelocity = Vector3.zero;
            this.jumpVelocity.y = this.jumpSpeed;
            this.jumpCooldownTimer = this.jumpCooldown;
            this.isJumping = true;
            JumpAudioSource.Play();
        }
        if (this.jumpVelocity.y > 0.0f)
        {
            this.jumpVelocity.y -= Time.fixedDeltaTime;
        }
        else
        {
            this.jumpVelocity = Vector3.zero;
        }
        this.jumpCooldownTimer -= Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetPlatformVelocity();
        this.HandleJumping();
        var inputMovement = this.moveAction.ReadValue<Vector2>();
        this.SetAnimationState(inputMovement);
        var inputRightDirection = this.cameraTransform.right;
        var inputForwardDirection = this.cameraTransform.forward;
        inputRightDirection.y = 0.0f;
        inputForwardDirection.y = 0.0f;
        inputRightDirection.Normalize();
        inputForwardDirection.Normalize();
        //Since we do not use the physics system, we have to simulate gravity ourselves
        if (this.controller.isGrounded)
        {
            this.characterGravity.y = 0.0f;
        }
        this.characterGravity.y += this.gravity * Time.fixedDeltaTime;
        this.characterMovement += this.characterGravity * Time.fixedDeltaTime;
        this.characterMovement += this.jumpVelocity * Time.fixedDeltaTime;
        this.characterMovement += inputRightDirection * inputMovement.x * this.characterSpeed * Time.fixedDeltaTime;
        this.characterMovement += inputForwardDirection * inputMovement.y * this.characterSpeed * Time.fixedDeltaTime;
        this.characterMovement *= (1 - this.dampening);
        Vector3 characterForward = this.characterMovement;
        characterForward.y = 0.0f;
        if (characterForward.sqrMagnitude > 0.0f && characterForward != Vector3.zero && !this.isDeath)
        {
            this.transform.forward = characterForward.normalized;
        }
        var combinedMovement = this.characterMovement + this.platformVelocity * Time.fixedDeltaTime;
        if(!this.isDeath)
            this.controller.Move(combinedMovement);

        if(inputMovement != Vector2.zero && !isJumping)
        {
            this.footstepAudioSource.mute = false;
            if (!dustParticles.isPlaying)
                dustParticles.Play();
        }
        else
        {
            this.footstepAudioSource.mute = true;
            if (dustParticles.isPlaying)
                dustParticles.Stop();
        }

        if(this.currentHealth <= 0)
        {
            this.isDeath = true;
            SetAnimationState(inputMovement);
        }
        else
        {
            this.isDeath = false;
        }
    }

    private void GetPlatformVelocity()
    {
        int platformLayer = LayerMask.GetMask("Platforms");

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.1f, platformLayer))
        {
            if(hit.collider.TryGetComponent<MovingPlatform>(out MovingPlatform platform))
            {
                platformVelocity = platform.GetVelocity();
            }
            else
            {
                platformVelocity = Vector3.zero;
            }
        }
        else
        {
            platformVelocity = Vector3.zero;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy"))
        {
            if (IsStompingEnemy() && isJumping)
            {
                if (hit.gameObject.TryGetComponent<Skeleton>(out var enemy))
                {
                    enemy.GetHit();
                }
            }
        }
    }

    private bool IsStompingEnemy()
    {
        // Cast a ray downward from the player's feet
        float rayLength = 0.5f;
        int enemyLayer = LayerMask.GetMask("Enemy");

        bool enemyBelow = Physics.Raycast(
            transform.position,   // from player center
            Vector3.down,         // downward
            rayLength,            // short distance
            enemyLayer            // only hit Enemy layer
        );

        return enemyBelow;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
