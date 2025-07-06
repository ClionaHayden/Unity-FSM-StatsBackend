using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton instance for global access
    public static AudioManager Instance;

    // AudioSource component for playing background music
    public AudioSource musicSource;
    
    // AudioSource component for playing sound effects (SFX)
    public AudioSource sfxSource;

    // Different AudioClips for various sounds and music in the game
    public AudioClip mainMenuMusic;
    public AudioClip gameplayMusic;
    public AudioClip buttonClickSound;
    public AudioClip waveStartSound;
    public AudioClip waveEndSound;
    public AudioClip gameOverSound;
    public AudioClip DamageSound;
    public AudioClip attackSound;
    public AudioClip enemySpawnSound;

    void Awake()
    {
        // Implement Singleton pattern:
        // If no instance exists, assign this as the instance and make persistent
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps this object across scene loads
        }
        else
        {
            // If another instance already exists, destroy this duplicate
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Load saved volume settings or default to full volume (1f)
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    // Set and save music volume
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    // Set and save sound effects (SFX) volume
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    // Play a sound effect once without interrupting other SFX
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    // Play background music; if the clip is already playing, do nothing
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.loop = true; // Loop music indefinitely
        musicSource.Play();
    }

    // Stop playing background music
    public void StopMusic()
    {
        musicSource.Stop();
    }
}
