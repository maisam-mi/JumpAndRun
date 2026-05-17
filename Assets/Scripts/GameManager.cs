using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    [SerializeField] private GameObject character;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private Lever lever;
    [SerializeField] private GameObject key;
    [SerializeField] private GameObject jewel;
    [SerializeField] private GameObject heart;
    [SerializeField] private List<GameObject> coins;
    [SerializeField] private List<GameObject> skeletons;

    public bool IsDoorOpened { private set; get; } = false;
    private bool isWon = false;


    private void Awake()
    {
        instance = this;
    }

    public void SetDoorState(bool state)
    {
        IsDoorOpened = state;
    }

    public void SetIsWon(bool won)
    {
        isWon = won;
    }

    public void Restart()
    {
        // respawn the character
        CharacterController controller = character.GetComponent<CharacterController>();
        controller.enabled = false;
        controller.transform.position = respawnPoint.position;
        controller.enabled = true;

        // close the door and reappear the key.
        SetDoorState(false);
        key.SetActive(true);

        // reappear the coins
        foreach (GameObject coin in coins)
        {
            coin.SetActive(true);
        }

        // reappear the skeletons
        foreach (GameObject skeleton in skeletons)
        {
            skeleton.SetActive(true);
        }

        // reappear the jewel
        jewel.SetActive(true);

        // reappear the heart
        heart.SetActive(true);

        // reset the coin counter and timer
        UIManager.Instance.ResetUI(isWon);

        // health restored to 100%
        character.GetComponent<Character>().ResetHealth();

        // set the lever off.
        lever.ResetLever();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
