using UnityEngine;
using TMPro;

public class UITime : MonoBehaviour
{
    public TMP_Text timerText;
    public float startTimeInSeconds = 200f;  // Set to 200 seconds
    private float timeRemaining;
    private bool timerRunning = true;

    void Start()
    {
        timeRemaining = startTimeInSeconds;
        UpdateTimerText();
    }

    void Update()
    {
        if (timerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerText();
            }
            else
            {
                timeRemaining = 0;
                timerRunning = false;
                Debug.Log("Time's up!");

                // Notify the Manager to show the lose screen
                Manager.Instance.HandleTimerExpiration();
            }
        }
    }

    public void UpdateTimerText()
    {
        timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
    }
}