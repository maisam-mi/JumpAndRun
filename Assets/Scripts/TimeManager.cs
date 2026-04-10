using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float ElapsedTime { get; private set; } = 0f;
    [SerializeField] private TMP_Text timerText;
    private bool gameRunning = true;

    // Update is called once per frame
    void Update()
    {
        if (gameRunning)
        {
            ElapsedTime += Time.deltaTime;
            float minutes = Mathf.FloorToInt(ElapsedTime / 60);
            float seconds = Mathf.FloorToInt(ElapsedTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        
    }

    public void SetGameRunning()
    {
        gameRunning = false;
    }
}
