using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Reference to the Pause Menu UI GameObject
    public GameObject pauseMenuUI;

    // Reference to the in-game HUD or gameplay UI GameObject
    public GameObject gameUI;

    // Tracks whether the game is currently paused
    private bool isPaused = false;

    void Update()
    {
        // Listen for Escape key press to toggle pause state
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();  // If paused, resume the game
            else
                Pause();   // If not paused, pause the game
        }
    }

    // Resumes gameplay from a paused state
    public void Resume()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSound);

        // Hide pause menu UI
        pauseMenuUI.SetActive(false);

        // Optionally disable all child elements under the pause menu object
        SetAllMenuElementsActive(this.gameObject, false);

        // Show gameplay UI
        gameUI.SetActive(true);

        // Optionally enable all child elements under the game UI object
        SetAllMenuElementsActive(gameUI, true);

        // Resume time scaling (normal time)
        Time.timeScale = 1f;

        isPaused = false;
    }

    // Pauses the game and shows the pause menu
    public void Pause()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSound);

        // Hide gameplay UI
        gameUI.SetActive(false);

        // Optionally disable all child elements under the gameplay UI object
        SetAllMenuElementsActive(gameUI, false);

        // Show pause menu UI
        pauseMenuUI.SetActive(true);

        // Optionally enable all child elements under the pause menu object
        SetAllMenuElementsActive(this.gameObject, true);

        // Freeze time in the game
        Time.timeScale = 0f;

        isPaused = true;
    }

    // Restarts the current level/scene
    public void Restart()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSound);

        // Resume normal time scale before loading scene
        Time.timeScale = 1f;

        // Reload the active scene by index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Quits to the main menu scene
    public void QuitGame()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSound);

        // Resume normal time scale before loading main menu
        Time.timeScale = 1f;

        // Load the main menu scene by name
        SceneManager.LoadScene("MainMenuScene");
    }

    // Helper function to enable or disable all child GameObjects of a parent
    void SetAllMenuElementsActive(GameObject parent, bool isActive)
    {
        foreach (Transform child in parent.transform)
        {
            child.gameObject.SetActive(isActive);
        }
    }
}
