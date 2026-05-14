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
    [SerializeField] private List<GameObject> coins;

    private bool isWon = false;


    private void Awake()
    {
        instance = this;
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

        // reset the coins
        foreach (GameObject coin in coins)
        {
            coin.SetActive(true);
        }

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
