using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{

    [SerializeField] private TimeManager timeManager;
    [SerializeField] private TMP_Text infoText;
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
        timeManager.SetGameRunning();
        infoText.gameObject.SetActive(true);
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
