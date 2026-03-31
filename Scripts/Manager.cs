using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set; }

    [SerializeField] GameObject Player;
    [SerializeField] GameObject pauseScreen; // Reference to the pause screen panel
    [SerializeField] GameObject winScreen;  // Reference to the win screen canvas
    [SerializeField] GameObject deathScreen; // Reference to the death screen canvas

    private bool isPaused = false; // Tracks if the game is paused
    private bool timerExpired = false; // Tracks if the timer has run out
    private int lives = 3;
    private int coins;

    public int coinValue;
    public static bool hasKey = false;

    Vector3 lastCheckpointPosition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        // Automatically reassign references after DontDestroyOnLoad
        ReassignReferences();
    }

    private void ReassignReferences()
    {
        // Find references to important GameObjects and UI elements
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");

        if (pauseScreen == null)
            pauseScreen = GameObject.FindGameObjectWithTag("PauseScreen");

        if (winScreen == null)
            winScreen = GameObject.FindGameObjectWithTag("WinScreen");

        if (deathScreen == null)
            deathScreen = GameObject.FindGameObjectWithTag("DeathScreen");
    }

    private void ResetManagerState()
    {
        coins = 0;
        hasKey = false;
        timerExpired = false;
        lastCheckpointPosition = Vector3.zero;

        if (pauseScreen != null) pauseScreen.SetActive(false);
        if (winScreen != null) winScreen.SetActive(false);
        if (deathScreen != null) deathScreen.SetActive(false);
    }

    void Start()
    {
        lastCheckpointPosition = transform.position;
        Debug.Log("Lives: " + lives);

        if (pauseScreen != null) pauseScreen.SetActive(false); // Ensure pause screen starts inactive
        if (winScreen != null) winScreen.SetActive(false); // Ensure win screen starts inactive
        if (deathScreen != null) deathScreen.SetActive(false); // Ensure death screen starts inactive
    }

    void Update()
    {
        // Check for Escape key press to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseButton();
        }
    }

    public void ImmidiateRestart()
    {
        lives -= 1;
        Player.transform.position = lastCheckpointPosition;
    }

    public void AddCoin(int coinValue)
    {
        coins += coinValue;
        print("Coins = " + coins);
        UpdateUICoinText();
    }

    public int GetCoins()
    {
        return coins;
    }

    public void UpdateUICoinText()
    {
        UICoin coinUI = FindObjectOfType<UICoin>();
        if (coinUI != null)
        {
            coinUI.UpdateCoinText();
        }
    }

    public static void KeyPickup()
    {
        hasKey = true;
        Debug.Log("Player has the key: " + hasKey);
    }

    public void UpdateCheckpoints(GameObject lastCheckpoint)
    {
        lastCheckpointPosition = lastCheckpoint.transform.position;
        Checkpoint[] allCheckpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);

        foreach (Checkpoint cp in allCheckpoints)
        {
            if (cp.gameObject != lastCheckpoint)
            {
                cp.MakeInactive();
            }
        }
    }

    public void ActivateWinScreen()
    {
        if (winScreen != null)
        {
            Time.timeScale = 0; // Pause the game
            winScreen.SetActive(true);
        }
        else
        {
            Debug.LogError("Win screen is not assigned in the Manager!");
        }
    }

    public void ActivateDeathScreen()
    {
        if (deathScreen != null)
        {
            Time.timeScale = 0; // Pause the game
            deathScreen.SetActive(true);
        }
        else
        {
            Debug.LogError("Death screen is not assigned in the Manager!");
        }
    }

    public void HandleTimerExpiration()
    {
        if (!timerExpired)
        {
            timerExpired = true;
            ActivateDeathScreen();
        }
    }

    public void PlayButton()
    {
        
        Time.timeScale = 1; // Resume time in case it's paused
        SceneManager.LoadScene(0); // Load the first scene
    }

    public void QuitButton()
    {
        Debug.Log("Game is quitting...");
        Application.Quit();
    }

    public void PauseButton()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0; // Pause the game
            if (pauseScreen != null) pauseScreen.SetActive(true); // Activate the pause screen
        }
        else
        {
            Time.timeScale = 1; // Resume the game
            if (pauseScreen != null) pauseScreen.SetActive(false); // Deactivate the pause screen
        }
    }
}