using UnityEngine;
using TMPro;  // Use TMPro for TextMeshPro UI text, or use UnityEngine.UI if using regular Text

public class WaveStatusUI : MonoBehaviour
{
    // Reference to the UI Text element displaying wave messages
    public TMP_Text waveText;  // Assign this in the inspector

    // Coroutine handle used to clear the text after a delay
    private Coroutine clearCoroutine;

    private void Start()
    {
        // Initialize the waveText to empty when the game starts
        waveText.text = "";
    }

    // Call this to display a "wave started" message with the wave number
    public void ShowWaveStart(int waveNumber)
    {
        ShowMessage($"Wave {waveNumber} started!");
    }

    // Call this to display a "wave completed" message with the wave number
    public void ShowWaveEnd(int waveNumber)
    {
        ShowMessage($"Wave {waveNumber} completed!");
    }

    // Internal method to display a message and optionally clear previous coroutines
    private void ShowMessage(string message)
    {
        // If a clear coroutine is already running, stop it to reset timer
        if (clearCoroutine != null)
            StopCoroutine(clearCoroutine);

        // Set the waveText to the new message
        waveText.text = message;

        // Uncomment below to clear the message automatically after 3 seconds
        // clearCoroutine = StartCoroutine(ClearAfterSeconds(3f));
    }

    // Coroutine to clear the waveText after a delay (in seconds)
    private System.Collections.IEnumerator ClearAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        waveText.text = "";
    }
}
