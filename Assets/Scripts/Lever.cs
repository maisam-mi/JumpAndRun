using UnityEngine;
using UnityEngine.InputSystem;

public class Lever : MonoBehaviour
{

    [SerializeField] Transform onPosition;
    [SerializeField] Transform offPosition;
    [SerializeField] GameObject leverHandle;

    private bool isOn = false;
    private bool isInterpolating = false;
    private InputAction interactAction;

    void Start()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    void ToggleLever()
    {
        isOn = !isOn;
        isInterpolating = true;
        float cooldown = 0.5f; // Duration of the interpolation
        Vector3 startPosition, targetPosition;
        Quaternion startRotation, targetRotation;

        if (isOn)
        {
            startPosition = onPosition.position;
            targetPosition = offPosition.position;
            startRotation = onPosition.rotation;
            targetRotation = offPosition.rotation;
        }
        else
        {
            startPosition = offPosition.position;
            targetPosition = onPosition.position;
            startRotation = offPosition.rotation;
            targetRotation = onPosition.rotation;
        }
        leverHandle.transform.SetPositionAndRotation(targetPosition, targetRotation);
        isInterpolating = false;
    }

    void Update()
    {
        if (interactAction.WasPressedThisFrame())
        {
            ToggleLever();
            Debug.Log("Lever interacted with!");
        }
    }
}
