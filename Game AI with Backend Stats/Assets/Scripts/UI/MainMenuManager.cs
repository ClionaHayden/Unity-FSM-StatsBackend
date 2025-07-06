using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    // References to the different UI panels in the main menu
    public GameObject optionsPanel;
    public GameObject creditsPanel;
    public GameObject controlsPanel;
    public GameObject leaderboardPanel;

    void Start()
    {
        // Play main menu background music when the scene starts
        AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenuMusic);
    }
    
    // Called when player clicks the Start button
    public void StartGame()
    {
        // Switch to gameplay music
        AudioManager.Instance.PlayMusic(AudioManager.Instance.gameplayMusic);

        // Load the game scene by name
        SceneManager.LoadScene("GameScene");
    }

    // Called when player opens the Options menu
    public void OpenOptions()
    {
        // Play button click sound effect
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSound);

        // Close any open panels before opening Options
        CloseAllPanels();

        // Show the Options panel if assigned
        if (optionsPanel != null) optionsPanel.SetActive(true);
    }

    // Called when player opens the Credits menu
    public void OpenCredits()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSound);
        CloseAllPanels();
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    // Called when player opens the Controls menu
    public void OpenControls()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSound);
        CloseAllPanels();
        if (controlsPanel != null) controlsPanel.SetActive(true);
    }

    // Called when player opens the Leaderboard menu
    public void OpenLeaderboard()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSound);
        CloseAllPanels();
        if (leaderboardPanel != null) leaderboardPanel.SetActive(true);
    }

    // Called when player clicks Quit button
    public void QuitGame()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSound);
        Debug.Log("Quitting game...");
        Application.Quit();  // Quit the application (works in built game, not editor)
    }

    // Called to return back to the main menu (e.g. from Options or other panels)
    public void BackToMainMenu()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSound);

        // Close all panels to show main menu UI
        CloseAllPanels();

        // Ensure this MainMenuManager GameObject is active
        this.gameObject.SetActive(true);
    }

    // Helper method to close all sub-panels in the menu
    private void CloseAllPanels()
    {
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (controlsPanel != null) controlsPanel.SetActive(false);
        if (leaderboardPanel != null) leaderboardPanel.SetActive(false);
    }
}
