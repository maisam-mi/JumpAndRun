using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.UIElements;

public class Sign : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private LocalizedString dialogueText;

    private bool canInteract;
    private InputAction inputAction;

    private void Start()
    {
        this.inputAction = InputSystem.actions.FindAction("Attack");
        this.inputAction.performed += ToggleDialogueBox;
    }

    private void ToggleDialogueBox(InputAction.CallbackContext context)
    {
        if (this.canInteract)
        {
            if(this.dialogueBox.activeInHierarchy)
            {
                this.dialogueBox.SetActive(false);
            }
            else
            {
                this.dialogueBox.SetActive(true);
                var uiDocument = this.dialogueBox.GetComponent<UIDocument>();
                var label = uiDocument.rootVisualElement.Q<Label>();
                label.text = this.dialogueText.GetLocalizedString();
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        this.canInteract = true;

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        this.canInteract = false;
    }
}
