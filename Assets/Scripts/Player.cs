using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Public Fields

    public float moveSpeed;
    public float poweredUpTime = 10.0f;

    public float poweredUpScaleX = 2.0f;

    public Vector2 movementRangeSmall;
    public Vector2 movementRangeLarge;

    #endregion

    #region Private Fields

    private bool isPoweredUp = false;

    private float powerTimer = 0.0f;

    private InputAction moveAction;
    private Rigidbody rb;

    #endregion

    void Start()
    {
        this.rb = this.GetComponentInChildren<Rigidbody>();
        this.moveAction = InputSystem.actions.FindAction("Move");
    }

    void FixedUpdate()
    {
        var movementRange = this.movementRangeSmall;

        float currentX = this.transform.position.x;
        Vector2 movement = this.moveAction.ReadValue<Vector2>();
        currentX += movement.x * this.moveSpeed * Time.fixedDeltaTime;
        currentX = Mathf.Clamp(currentX, movementRange.x, movementRange.y);

        var newPosition = new Vector3(currentX, this.transform.position.y, this.transform.position.z);
        this.rb.MovePosition(newPosition);
    }

    public void DeactivtePowerup()
    {

    }

    public void ActivatePowerup()
    {

    }
}
