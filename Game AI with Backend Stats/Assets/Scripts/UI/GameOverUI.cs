using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    // Reference to the Game Over panel UI GameObject
    public GameObject panel;

    private void Start()
    {
        // Hide the Game Over panel at the start of the game
        if (panel != null)
            panel.SetActive(false);
    }

    // Show the Game Over panel and pause the game
    public void ShowGameOver()
    {
        if (panel != null)
        {
            // Play game over sound effect
            AudioManager.Instance.PlaySFX(AudioManager.Instance.gameOverSound);
            
            // Activate the Game Over UI panel
            panel.SetActive(true);
            
            // Pause the game by stopping time
            Time.timeScale = 0f;
        }
    }

    // Called when the player clicks the Restart button
    public void OnRestartButton()
    {
        // Play button click sound effect
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSound);
        
        // Resume the game time
        Time.timeScale = 1f;
        
        // Reload the current active scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Called when the player clicks the Main Menu button
    public void OnMainMenuButton()
    {
        // Play button click sound effect
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSound);
        
        // Play main menu background music
        AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenuMusic);
        
        // Resume the game time in case it was paused
        Time.timeScale = 1f;
        
        // Load the Main Menu scene by name
        SceneManager.LoadScene("MainMenuScene");
    }
}
