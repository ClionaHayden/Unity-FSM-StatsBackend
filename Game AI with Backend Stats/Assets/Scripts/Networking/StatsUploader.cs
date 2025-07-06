using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

/// <summary>
/// Data class to represent player statistics to be uploaded.
/// </summary>
[System.Serializable]
public class PlayerStats {
    public int playerId;
    public int enemiesDefeated;
    public int damageDealt;
}

/// <summary>
/// Handles uploading player stats to a backend server, with retry, caching, and offline handling.
/// </summary>
public class StatsUploader : MonoBehaviour
{
    private const string CachedStatsKey = "CachedPlayerStats"; // Key for storing unsent stats locally

    public string apiUrl = "http://localhost:5000";             // Base URL for backend API
    public PlayerController playerController;                  // Reference to PlayerController for accessing stats
    public int maxRetries = 5;                                 // How many times to retry sending stats
    public float delayBetweenRetries = 10f;                    // Delay between retries
    public PlayerStatsUI uploadStatusUI;                       // UI element to show upload status messages

    private void Start()
    {
        // Automatically find player controller if not set
        if (playerController == null)
            playerController = FindFirstObjectByType<PlayerController>();

        if (playerController != null)
            playerController.OnPlayerDeath += HandlePlayerDeath; // Listen for player death to trigger upload
        else
            Debug.Log("Cannot find PlayerController");

        // Get API URL from PlayerPrefs (can be set in an options menu)
        apiUrl = PlayerPrefs.GetString("BackendURL", "http://localhost:5000") + "/PlayerStats";

        // Begin checking for connectivity
        StartCoroutine(NetworkCheckRoutine());

        // Try to send any previously cached stats
        StartCoroutine(TrySendCachedStats());
    }

    void Awake()
    {
        // Ensure this object persists across scenes
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Triggers stat upload (can be called externally).
    /// </summary>
    public void UploadStats()
    {
        StartCoroutine(SendStatsWithRetries());
    }

    /// <summary>
    /// Checks periodically if internet is available and tries to resend any cached stats.
    /// </summary>
    private IEnumerator NetworkCheckRoutine()
    {
        while (true)
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                if (PlayerPrefs.HasKey(CachedStatsKey))
                {
                    Debug.Log("Internet available, trying to resend cached stats...");
                    yield return TrySendCachedStats();
                }
            }
            else
            {
                Debug.Log("No internet connection.");
            }

            yield return new WaitForSeconds(delayBetweenRetries);
        }
    }

    /// <summary>
    /// Tries to send the current player's stats to the server, retrying if it fails.
    /// </summary>
    private IEnumerator SendStatsWithRetries()
    {
        if (playerController == null)
        {
            Debug.LogError("PlayerController reference not found.");
            yield break;
        }

        // Prepare the stats object and serialize to JSON
        PlayerStats pStats = new PlayerStats
        {
            playerId = playerController.playerId,
            enemiesDefeated = playerController.enemiesDefeated,
            damageDealt = playerController.damageDealt
        };

        string json = JsonUtility.ToJson(pStats);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        int attempt = 0;
        bool success = false;

        uploadStatusUI.ShowStatus("Uploading stats...");

        while (attempt < maxRetries && !success)
        {
            attempt++;
            Debug.Log($"Attempt {attempt} to send player stats...");

            UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Stats sent successfully");
                success = true;
                uploadStatusUI.ShowStatus("Upload successful!");
            }
            else
            {
                Debug.LogError("Error sending stats: " + request.error);
                uploadStatusUI.ShowStatus($"Upload failed, retrying... ({attempt}/{maxRetries})");

                if (attempt < maxRetries)
                    yield return new WaitForSeconds(delayBetweenRetries);
            }
        }

        // After all attempts failed, cache the stats locally
        if (!success)
        {
            Debug.LogError("Failed to send player stats after max retries.");
            PlayerPrefs.SetString(CachedStatsKey, json);
            PlayerPrefs.Save();
            uploadStatusUI.ShowStatus("Upload failed. Stats saved locally.");
        }
    }

    /// <summary>
    /// Attempts to resend any previously cached stats if they exist.
    /// </summary>
    private IEnumerator TrySendCachedStats()
    {
        if (PlayerPrefs.HasKey(CachedStatsKey))
        {
            string cachedJson = PlayerPrefs.GetString(CachedStatsKey);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(cachedJson);

            UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log("Trying to send cached stats...");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Cached stats sent successfully");
                PlayerPrefs.DeleteKey(CachedStatsKey);
            }
            else
            {
                Debug.LogWarning("Failed to send cached stats: " + request.error);
            }
        }
        else
        {
            Debug.Log("No cached stats found.");
        }
    }

    /// <summary>
    /// Sends a GET request to retrieve stats for a specific player.
    /// </summary>
    public IEnumerator GetPlayerStats(int playerId)
    {
        string getUrl = $"{apiUrl}/{playerId}";
        UnityWebRequest request = UnityWebRequest.Get(getUrl);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            PlayerStats stats = JsonUtility.FromJson<PlayerStats>(json);
            Debug.Log($"Received stats: EnemiesDefeated={stats.enemiesDefeated}, DamageDealt={stats.damageDealt}");
        }
        else
        {
            Debug.LogError("Error getting stats: " + request.error);
        }
    }

    /// <summary>
    /// Dev/test shortcut to trigger upload (S) or GET (G) via keyboard.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(SendStatsWithRetries());
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(GetPlayerStats(3));
        }
    }

    /// <summary>
    /// Clean up when object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        if (playerController != null)
            playerController.OnPlayerDeath -= HandlePlayerDeath;
    }

    /// <summary>
    /// Automatically triggered when the player dies.
    /// </summary>
    private void HandlePlayerDeath()
    {
        Debug.Log("Uploading stats due to player death...");
        StartCoroutine(SendStatsWithRetries());
    }
}
