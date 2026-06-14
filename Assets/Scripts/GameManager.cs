using UnityEngine;
using TMPro; // TextMeshPro integration
using UnityEngine.SceneManagement; // Required for reloading scenes

public class GameManager : MonoBehaviour
{
    // Singleton Pattern Instance
    public static GameManager instance;

    [Header("UI Settings")]
    public TextMeshProUGUI scoreText; // Drag your Score Text UI element here
    public GameObject gameOverPanel; // Drag your GameOverPanel GameObject here
    public GameObject winPanel;      // Drag your 'Mission Accomplished' panel here

    [Header("Score Settings")]
    public int totalScore = 0; // Tracks the current score

    [Header("Door Settings")]
    public GameObject exitDoor;    // Drag your hidden door here (starts hidden)
    public int starsToWin = 3;      // Number of stars required to reveal the door and win

    [Header("Spawning Settings")]
    public GameObject starPrefab; // Drag your Star prefab here
    public Transform[] spawnPositions; // Array holding the different spawn points in the scene

    [Header("Wall Avoidance (Optional)")]
    [Tooltip("Layers representing walls. Leave as Nothing if not using wall checking.")]
    public LayerMask blockedLayers;
    [Tooltip("Radius around the spawn point to check for walls.")]
    public float checkRadius = 0.3f;

    private void Awake()
    {
        // Singleton Implementation
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Safety Copy: Transfer Level 2 Inspector references to the persistent Level 1 instance
            instance.scoreText = this.scoreText;
            instance.gameOverPanel = this.gameOverPanel;
            instance.winPanel = this.winPanel;
            instance.exitDoor = this.exitDoor;
            instance.spawnPositions = this.spawnPositions;
            instance.starPrefab = this.starPrefab;
            instance.blockedLayers = this.blockedLayers;
            instance.checkRadius = this.checkRadius;
            instance.starsToWin = this.starsToWin;

            // Re-initialize scene items for the new level
            instance.InitializeNewLevel();

            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize the UI text and panel visibility for Level 1
        InitializeNewLevel();
    }

    // Resets score, updates UI, and hides panels/doors at the start of a level
    private void InitializeNewLevel()
    {
        totalScore = 0; // Reset score for the new level
        UpdateScoreUI();

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (exitDoor != null) exitDoor.SetActive(false);
    }

    // Adds score, updates UI, checks for door appearance, and triggers next star spawn
    public void AddScore(int amount)
    {
        totalScore += amount;
        UpdateScoreUI();

        // Check if score is enough to make the exit door appear
        if (totalScore >= starsToWin)
        {
            if (exitDoor != null)
            {
                exitDoor.SetActive(true); // Make the door appear
                Debug.Log("The hidden Exit Door has appeared!");
            }
        }

        SpawnNewStar();
    }

    // Public method to safely update the UI text and log details
    public void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + totalScore.ToString();
            Debug.Log("UI Updated! New Score: " + totalScore);
        }
        else 
        {
            Debug.LogWarning("Score Text is missing in GameManager!");
        }
    }

    // Activates the winPanel and stops time
    public void WinGame()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true); // Activate the 'Mission Accomplished' Panel
        }
        
        Time.timeScale = 0f; // Stop the game
        Debug.Log("Victory! Player successfully reached the exit door.");
    }

    // Activates the Game Over panel and freezes the game
    public void GameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // Activate the Game Over Panel
        }
        
        Time.timeScale = 0f; // Freeze the game
        Debug.Log("Game Over! Player caught by the enemy.");
    }

    // Unfreezes the game and reloads the current active scene by name
    public void RestartGame()
    {
        Time.timeScale = 1f; // Unfreeze time so the game can run again
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
    }

    // Unfreezes the game and loads the next scene index in Build Settings
    public void LoadNextLevel()
    {
        Time.timeScale = 1f; // Reset time scale so the next level is unfrozen
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Ensure we don't try to load beyond our available scenes
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("GameManager: No next level found in Build Settings!");
        }
    }

    // Safely spawns a new star at a random location
    public void SpawnNewStar()
    {
        if (starPrefab == null)
        {
            Debug.LogWarning("GameManager: Cannot spawn new star because 'starPrefab' is null!");
            return;
        }

        if (spawnPositions == null || spawnPositions.Length == 0)
        {
            Debug.LogWarning("GameManager: Cannot spawn new star because 'spawnPositions' array is null or empty!");
            return;
        }

        Transform selectedSpawnPoint = null;

        if (blockedLayers != 0)
        {
            for (int i = 0; i < 15; i++)
            {
                Transform candidate = spawnPositions[Random.Range(0, spawnPositions.Length)];
                if (candidate != null && !Physics2D.OverlapCircle(candidate.position, checkRadius, blockedLayers))
                {
                    selectedSpawnPoint = candidate;
                    break;
                }
            }
        }

        if (selectedSpawnPoint == null)
        {
            int randomIndex = Random.Range(0, spawnPositions.Length);
            selectedSpawnPoint = spawnPositions[randomIndex];
        }

        if (selectedSpawnPoint != null)
        {
            Instantiate(starPrefab, selectedSpawnPoint.position, Quaternion.identity);
        }
    }
}