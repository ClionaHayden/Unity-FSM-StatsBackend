using UnityEngine;

public class WeaponSlash : MonoBehaviour
{
    // The angle in degrees that the weapon swings during the slash
    public float slashAngle = 90f;

    // Speed multiplier for how fast the slash animation plays
    public float slashSpeed = 10f;

    // Reference to the player's transform for determining slash direction
    public Transform player;

    // Flag to track whether the weapon is currently slashing
    private bool isSlashing = false;

    // Original (rest) rotation of the weapon, used to reset after slash
    private Quaternion originalRotation;

    // Target rotation representing the end angle of the slash
    private Quaternion targetRotation;

    // Collider attached to the weapon, used to detect hits during slash
    private Collider2D weaponCollider;

    void Start()
    {
        // Set original rotation to identity (no rotation)
        originalRotation = Quaternion.identity;

        // Get the Collider2D component on this weapon object
        weaponCollider = GetComponent<Collider2D>();

        // Disable the collider initially, so it only detects hits during the slash
        if (weaponCollider != null)
            weaponCollider.enabled = false;
    }

    void Update()
    {
        // When left mouse button is pressed and not already slashing, start slash coroutine
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isSlashing)
        {
            StartCoroutine(Slash());
        }
    }

    // Coroutine that performs the slashing animation and damage detection
    System.Collections.IEnumerator Slash()
    {
        isSlashing = true;

        // Determine slash direction based on weapon relative to player
        // If weapon is to the right of player, slash direction is positive (clockwise)
        // Otherwise, slash direction is negative (counter-clockwise)
        float direction = transform.position.x >= player.position.x ? 1f : -1f;

        // Starting rotation (no rotation)
        Quaternion startRot = Quaternion.identity;

        // End rotation rotated by slashAngle in appropriate direction on Z axis
        Quaternion endRot = Quaternion.Euler(0, 0, -slashAngle * direction);

        // Enable weapon collider to detect hits at the start of the slash
        if (weaponCollider != null)
            weaponCollider.enabled = true;

        float time = 0;

        // Rotate weapon from start to end rotation over time
        while (time < 1f)
        {
            time += Time.deltaTime * slashSpeed;
            transform.localRotation = Quaternion.Lerp(startRot, endRot, time);
            yield return null; // Wait until next frame
        }

        // Disable collider once slash forward motion completes
        if (weaponCollider != null)
            weaponCollider.enabled = false;

        time = 0;

        // Rotate weapon back from end rotation to start rotation over time
        while (time < 1f)
        {
            time += Time.deltaTime * slashSpeed;
            transform.localRotation = Quaternion.Lerp(endRot, startRot, time);
            yield return null;
        }

        // Ensure rotation resets exactly to original (identity)
        transform.localRotation = startRot;

        // Slash complete, reset flag
        isSlashing = false;
    }

    // Called automatically when the weapon collider hits another collider
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to an enemy
        if (other.CompareTag("Enemy"))
        {
            // Try to get the EnemyAI script from the enemy object
            EnemyAI enemy = other.GetComponent<EnemyAI>();

            // If enemy script exists, apply damage
            if (enemy != null)
            {
                enemy.TakeDamage(1);
            }
        }
    }
}
