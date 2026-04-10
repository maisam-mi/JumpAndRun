using TMPro;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    public int Coins { get; private set; } = 0;

    [SerializeField] private TMP_Text scoreText;

    public void IncreaseCoins()
    {
        Coins += 1;
    }

    public void UpdateCoinsText()
    {
        scoreText.text = $"COINS: {Coins} / 10";
    }
}
