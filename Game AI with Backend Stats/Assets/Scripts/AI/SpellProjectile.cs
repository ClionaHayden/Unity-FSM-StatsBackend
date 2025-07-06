using UnityEngine;

/// <summary>
/// Handles the behavior of a spell projectile: movement, damage, and collision.
/// </summary>
public class SpellProjectile : MonoBehaviour
{
    public float speed = 5f;         // Movement speed of the projectile
    public int damage = 1;           // Damage dealt to the player on hit
    public float lifetime = 3f;      // Time before the projectile is auto-destroyed

    private Vector3 direction;       // Direction the projectile will travel in

    /// <summary>
    /// Called externally to set up the projectile's direction and rotation toward a target.
    /// </summary>
    /// <param name="targetPosition">The world position to aim at.</param>
    public void Initialize(Vector3 targetPosition)
    {
        // Play attack sound effect when spell is cast
        AudioManager.Instance.PlaySFX(AudioManager.Instance.attackSound);

        // Calculate normalized direction to target
        direction = (targetPosition - transform.position).normalized;

        // Rotate the projectile to face the movement direction (z-axis)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

        // Auto-destroy the projectile after a set time
        Destroy(gameObject, lifetime);
    }

    /// <summary>
    /// Moves the projectile forward every frame.
    /// </summary>
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    /// <summary>
    /// Handles collision logic when the projectile hits something.
    /// </summary>
    /// <param name="other">The collider it hits.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Attempt to damage the player
            var pHealth = other.GetComponent<PlayerController>();
            if (pHealth != null)
            {
                pHealth.TakeDamage(damage);
            }

            // Destroy projectile after hitting player
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Enemy")) // Avoid destroying on friendly fire (enemy vs enemy)
        {
            // Destroy on collision with walls, objects, etc.
            Destroy(gameObject);
        }
    }
}
