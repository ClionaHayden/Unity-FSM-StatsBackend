using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

// Serializable class to store player statistics fetched from backend
[System.Serializable]
public class PlayerStat
{
    public int playerId;          // Unique ID of the player
    public int enemiesDefeated;   // Number of enemies defeated by player
    public int damageDealt;       // Total damage dealt by player
}

public class LeaderboardUI : MonoBehaviour
{
    // Key to retrieve backend URL from PlayerPrefs
    private const string BackendUrlKey = "BackendURL";

    // Base URL for leaderboard API; default to localhost
    public string leaderboardUrl = "http://localhost:5000";

    // Parent transform where leaderboard entries will be instantiated
    public Transform contentPanel;

    // Prefab for individual leaderboard entry UI elements
    public GameObject entryPrefab;

    private void Start()
    {
        // Retrieve backend URL from PlayerPrefs or use default, then append endpoint
        leaderboardUrl = PlayerPrefs.GetString(BackendUrlKey, "http://localhost:5000") + "/PlayerStats";

        // Start coroutine to fetch and display leaderboard data
        StartCoroutine(LoadLeaderboard());
    }

    // Coroutine to request leaderboard data asynchronously
    IEnumerator LoadLeaderboard()
    {
        UnityWebRequest request = UnityWebRequest.Get(leaderboardUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Deserialize JSON response into PlayerStat array
            PlayerStat[] players = JsonHelper.FromJson<PlayerStat>(request.downloadHandler.text);

            // Populate UI with player data as a List
            PopulateUI(players.ToList());
        }
        else
        {
            // Log error if the request failed
            Debug.LogError("Error fetching leaderboard: " + request.error);
        }
    }

    // Populate leaderboard UI with player stats
    void PopulateUI(List<PlayerStat> players)
    {
        // Remove existing leaderboard entries to avoid duplicates
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        // Process players:
        // - Group entries by playerId
        // - Select the entry with max enemiesDefeated per player
        // - Order all players by enemiesDefeated descending (highest first)
        var filteredPlayers = players
            .GroupBy(p => p.playerId)
            .Select(g => g.OrderByDescending(p => p.enemiesDefeated).First())
            .OrderByDescending(p => p.enemiesDefeated)
            .ToList();

        // Instantiate a UI entry for each filtered player and set text values
        foreach (var stat in filteredPlayers)
        {
            GameObject entry = Instantiate(entryPrefab, contentPanel);

            // Assumes the prefab has two TMP_Text components:
            // texts[0] for playerId, texts[1] for enemiesDefeated
            TMP_Text[] texts = entry.GetComponentsInChildren<TMP_Text>();
            texts[0].text = $"{stat.playerId}";
            texts[1].text = $"{stat.enemiesDefeated}";
        }
    }
}
