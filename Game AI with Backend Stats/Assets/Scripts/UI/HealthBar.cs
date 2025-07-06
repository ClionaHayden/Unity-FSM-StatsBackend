using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Reference to the UI Image that represents the health bar fill
    public Image fillImage; // assign the HealthBarFill Image here

    // Reference to the player controller to access the player's health
    public PlayerController playerController;

    // Store the player's max health for normalization
    private float maxHealth;

    void Start()
    {
        // If playerController is not assigned in inspector, find the first PlayerController in the scene
        if (playerController == null)
            playerController = FindFirstObjectByType<PlayerController>();

        // Initialize maxHealth from the player's current health value
        maxHealth = playerController.playerHealth;
    }

    void Update()
    {
        // Update the health bar fill only if references are set
        if (playerController != null && fillImage != null)
        {
            // Calculate fill amount as a ratio of current health to max health
            float fillAmount = playerController.playerHealth / maxHealth;

            // Update the health bar's vertical scale to reflect current health
            // Keeps width and depth the same, scales only height (Y-axis)
            fillImage.rectTransform.localScale = new Vector3(1f, fillAmount, 1f);
        }
    }
}
