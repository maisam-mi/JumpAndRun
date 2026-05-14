using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance;

    private PlayerStatistics statistics;

    [SerializeField] private Character character;
    [SerializeField] private Image healthBar;

    [SerializeField] private TextMeshProUGUI coinCounterText;
    [SerializeField] private TMP_Text timerText;

    [SerializeField] private TextMeshProUGUI victoryCoinCounterText;
    [SerializeField] private TMP_Text victoryTimerText;

    [SerializeField] private Canvas hudCanvas;
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private Canvas victoryCanvas;
    [SerializeField] private float fadingTime = 0.25f;
    private bool isFadingInGameOver = false;
    private bool gameRunning = true;
    private CanvasGroup hudCanvasGroup;
    private CanvasGroup gameOverCanvasGroup;
    private CanvasGroup victoryCanvasGroup;

    private IEnumerator FadeInVictory()
    {
        this.isFadingInGameOver = true;

        this.victoryCanvasGroup.interactable = true;

        float timer = 0.0f;
        while (timer < this.fadingTime)
        {
            float percent = timer / this.fadingTime;

            this.victoryCanvasGroup.alpha = percent;
            yield return null;
            timer += Time.deltaTime;
        }
        this.victoryCanvasGroup.alpha = 1.0f;
    }
    private IEnumerator FadeOutVictory()
    {
        this.isFadingInGameOver = false;

        this.victoryCanvasGroup.interactable = false;

        float timer = 0.0f;
        while (timer < this.fadingTime)
        {
            float percent = timer / this.fadingTime;

            this.victoryCanvasGroup.alpha = 1.0f - percent;
            yield return null;
            timer += Time.deltaTime;
        }
        this.victoryCanvasGroup.alpha = 0.0f;
    }

    private IEnumerator FadeInGameOver()
    {
        this.isFadingInGameOver = true;

        this.gameOverCanvasGroup.interactable = true;

        float timer = 0.0f;
        while (timer < this.fadingTime)
        {
            float percent = timer / this.fadingTime;
            
            this.gameOverCanvasGroup.alpha = percent;
            yield return null;
            timer += Time.deltaTime;
        }
        this.gameOverCanvasGroup.alpha = 1.0f;
    }

    private IEnumerator FadeOutGameOver()
    {
        this.isFadingInGameOver = false;

        this.gameOverCanvasGroup.interactable = false;

        float timer = 0.0f;
        while (timer < this.fadingTime)
        {
            float percent = timer / this.fadingTime;

            this.gameOverCanvasGroup.alpha = 1.0f - percent;
            yield return null;
            timer += Time.deltaTime;
        }
        this.gameOverCanvasGroup.alpha = 0.0f;
    }

    private IEnumerator FadeInUHD()
    {
        float timer = 0.0f;
        while (timer < this.fadingTime)
        {
            float percent = timer / this.fadingTime;

            this.hudCanvasGroup.alpha = percent;
            yield return null;
            timer += Time.deltaTime;
        }
        this.hudCanvasGroup.alpha = 1.0f;
    }

    private IEnumerator FadeOutUHD()
    {
        float timer = 0.0f;
        while (timer < this.fadingTime)
        {
            float percent = timer / this.fadingTime;

            this.hudCanvasGroup.alpha = 1.0f - percent;
            yield return null;
            timer += Time.deltaTime;
        }
        this.hudCanvasGroup.alpha = 0.0f;
    }


    private void Awake()
    {
        instance = this;
        this.statistics = new PlayerStatistics();

        this.hudCanvasGroup = hudCanvas.GetComponent<CanvasGroup>();
        this.gameOverCanvasGroup = gameOverCanvas.GetComponent<CanvasGroup>();
        this.victoryCanvasGroup = victoryCanvas.GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        float healthInPercent = this.character.GetCurrentHealth() / this.character.GetMaxHealth();
        this.healthBar.fillAmount = healthInPercent;

        if (gameRunning)
        {
            this.statistics.timer += Time.deltaTime;
            float minutes = Mathf.FloorToInt(this.statistics.timer / 60);
            float seconds = Mathf.FloorToInt(this.statistics.timer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        

        if (healthInPercent <= 0.0f && !this.isFadingInGameOver)
        {
            ShowGameOver();
        }
    }

    public void ResetUI(bool isWon)
    {
        if (isWon)
        {
            this.StartCoroutine(FadeOutVictory());
        }
        else
        {
            this.StartCoroutine(FadeOutGameOver());
        }

            this.StartCoroutine(FadeInUHD());

        this.statistics.coinCounter = 0;
        this.statistics.timer = 0.0f;

        float minutes = Mathf.FloorToInt(this.statistics.timer / 60);
        float seconds = Mathf.FloorToInt(this.statistics.timer % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        string coinText = $"Coins: {this.statistics.coinCounter}";
        coinCounterText.text = coinText;
    }

    public void ShowGameOver()
    {
        gameOverCanvas.GetComponent<Canvas>().sortingOrder = 1;
        victoryCanvas.GetComponent<Canvas>().sortingOrder = 0;
        GameManager.Instance.SetIsWon(false);

        this.StartCoroutine(FadeOutUHD());
        this.StartCoroutine(FadeInGameOver());
    }

    public void ShowVictory()
    {
        victoryCanvas.GetComponent<Canvas>().sortingOrder = 1;
        gameOverCanvas.GetComponent<Canvas>().sortingOrder = 0;
        
        SetGameRunning(false);

        float minutes = Mathf.FloorToInt(this.statistics.timer / 60);
        float seconds = Mathf.FloorToInt(this.statistics.timer % 60);
        victoryTimerText.text = $"Time Record: {string.Format("{0:00}:{1:00}", minutes, seconds)}";
        string coinText = $"Coins Collected: {this.statistics.coinCounter}";
        victoryCoinCounterText.text = coinText;

        this.StartCoroutine(FadeOutUHD());
        this.StartCoroutine(FadeInVictory());
    }

    public void CollectCoin()
    {
        this.statistics.coinCounter++;
        string coinText = $"Coins: {this.statistics.coinCounter}";
        coinCounterText.text = coinText;
    }

    public void SetGameRunning(bool running)
    {
        gameRunning = running;
    }

    // TODO: extract into own script
    private class PlayerStatistics
    {
        public int coinCounter = 0;
        public float timer = 0.0f;
        // add more statistics
    }
}
