using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    // Reference to the PlayerController script, holds player stats
    public PlayerController playerController;

    // Reference to the StatsUploader script, handles uploading stats
    public StatsUploader statsUploader;

    // UI text elements for displaying stats
    // public Text healthText; // Currently commented out
    public TMP_Text killsText;
    public TMP_Text damageText;

    // Text element to display status messages (e.g., uploading)
    public TMP_Text statusMessageText;

    // Button to trigger stats upload
    public Button uploadButton;

    void Start()
    {
        // Find PlayerController instance if not assigned
        if (playerController == null)
            playerController = FindFirstObjectByType<PlayerController>();

        // Find StatsUploader instance if not assigned
        if (statsUploader == null)
            statsUploader = FindFirstObjectByType<StatsUploader>();

        // Add listener to upload button to trigger stats upload and show status message
        uploadButton.onClick.AddListener(() =>
        {
            statsUploader.UploadStats();
            ShowStatus("Uploading stats...");
        });

        // Subscribe to player's stats change event and update UI initially
        if (playerController != null)
        {
            playerController.OnStatsChanged += Update;
            Update(); // Initial UI update
        }
    }

    // Updates the displayed stats UI values
    void Update()
    {
        if (playerController != null)
        {
            //healthText.text = $"Health: {playerController.playerHealth}"; // Commented out
            killsText.text = $"Kills: {playerController.enemiesDefeated}";
            damageText.text = $"Damage: {playerController.damageDealt}";
        }
    }

    // Shows a status message on screen for 3 seconds
    public void ShowStatus(string message)
    {
        statusMessageText.text = message;
        statusMessageText.gameObject.SetActive(true);
        Invoke(nameof(ClearStatus), 3f); // Automatically clear after 3 seconds
    }

    // Clears the status message text and hides the UI element
    void ClearStatus()
    {
        statusMessageText.gameObject.SetActive(false);
    }
    
    // Clean up subscription to prevent memory leaks or errors
    private void OnDestroy()
    {
        if (playerController != null)
            playerController.OnStatsChanged -= Update;
    }
}
