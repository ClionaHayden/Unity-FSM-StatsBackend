using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    // UI references for settings controls
    public Slider volumeSlider;          // Controls music volume
    public Slider sfxSlider;             // Controls sound effects volume
    public Toggle fullscreenToggle;      // Toggles fullscreen mode
    public TMP_InputField backendUrlInput;  // Input for backend URL

    // Key for saving/loading backend URL from PlayerPrefs
    private const string BackendUrlKey = "BackendURL";

    private void Start()
    {
        // Make sure AudioManager instance exists before proceeding
        if (AudioManager.Instance == null)
        {
            Debug.LogError("AudioManager not found!");
            return;
        }

        // Load saved settings or use defaults

        // Set music volume slider to saved music volume (default 1)
        volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);

        // Set SFX volume slider to saved SFX volume (default 1)
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Set fullscreen toggle based on current screen mode
        fullscreenToggle.isOn = Screen.fullScreen;

        // Load backend URL from PlayerPrefs or default to localhost
        string savedUrl = PlayerPrefs.GetString(BackendUrlKey, "http://localhost:5000");
        backendUrlInput.text = savedUrl;

        // Apply loaded music volume to the AudioListener (overall volume)
        AudioListener.volume = volumeSlider.value;

        // Register listeners for UI changes to update settings live

        volumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        backendUrlInput.onEndEdit.AddListener(SetBackendURL);
    }

    // Called when music volume slider is adjusted
    public void OnMusicVolumeChanged(float volume)
    {
        // Update AudioManager music volume and save setting
        AudioManager.Instance.SetMusicVolume(volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    // Called when SFX volume slider is adjusted
    public void OnSFXVolumeChanged(float volume)
    {
        // Update AudioManager SFX volume and save setting
        AudioManager.Instance.SetSFXVolume(volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    // Called when fullscreen toggle is changed
    public void SetFullscreen(bool isFullscreen)
    {
        // Set fullscreen mode on/off
        Screen.fullScreen = isFullscreen;
    }

    // Called when user finishes editing backend URL input field
    public void SetBackendURL(string url)
    {
        // Validate non-empty input and save new backend URL
        if (!string.IsNullOrWhiteSpace(url))
        {
            PlayerPrefs.SetString(BackendUrlKey, url);
            Debug.Log("Backend URL set to: " + url);
        }
    }

    // Helper method to get current backend URL saved in PlayerPrefs
    public string GetBackendURL()
    {
        return PlayerPrefs.GetString(BackendUrlKey, "http://localhost:5000");
    }
}
