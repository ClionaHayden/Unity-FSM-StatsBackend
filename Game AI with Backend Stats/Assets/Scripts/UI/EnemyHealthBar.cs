using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    // Reference to the UI Image component used as the health bar fill
    [SerializeField] private Image fillImage;

    // Reference to the main camera's transform, used for facing the health bar towards the camera
    private Transform cam;

    private void Start()
    {
        // Cache the main camera transform for performance
        cam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        // Rotate the health bar to always face the camera
        // Quaternion.LookRotation creates a rotation looking from the health bar to the camera
        transform.rotation = Quaternion.LookRotation(transform.position - cam.position);
    }

    // Update the fill amount of the health bar based on normalized health value (0 to 1)
    public void SetHealth(float normalized)
    {
        fillImage.fillAmount = normalized;
    }
}
