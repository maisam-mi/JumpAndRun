using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ResetGame : MonoBehaviour
{

    [SerializeField] private UIManager UIManager;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private LocalizedString dialogueText;
    private bool gameFinished = false;
    private InputAction clickAction;

    private void Start()
    {
        clickAction = InputSystem.actions.FindAction("Click");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        gameFinished = true;
        UIManager.SetGameRunning(false);
        this.dialogueBox.SetActive(true);
        var uiDocument = this.dialogueBox.GetComponent<UIDocument>();
        var label = uiDocument.rootVisualElement.Q<Label>();
        label.text = this.dialogueText.GetLocalizedString();
    }

    private void Update()
    {
        if (gameFinished)
        {
            if (clickAction.IsPressed())
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

}
