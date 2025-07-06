using System;
using UnityEngine;

/// <summary>
/// Controls player movement, health, and tracks gameplay stats like kills and damage dealt.
/// Raises events on death and stats changes.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;  // Player movement speed

    public int playerHealth = 10;                    // Player's starting health

    private Rigidbody2D rb;                           // Rigidbody for physics-based movement
    private Vector2 movement;                         // Current movement input
    private Camera mainCamera;                        // Reference to main camera for clamping position
    private SpriteRenderer spriteRenderer;           // Reference to sprite renderer for flipping sprite

    public int playerId = 3;                          // Player ID (for stats tracking)
    public int enemiesDefeated = 0;                   // Number of enemies defeated by player
    public int damageDealt = 0;                        // Total damage dealt by player

    // Event triggered when the player dies
    public event Action OnPlayerDeath;
    private bool isDead = false;                      // Flag to prevent multiple death triggers

    // Event triggered whenever player stats (kills, damage) change
    public event Action OnStatsChanged;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogWarning("Rigidbody2D component missing from the player!");

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        mainCamera = Camera.main;  // Cache main camera reference
    }

    private void Update()
    {
        // Read raw input (-1, 0, or 1) for horizontal and vertical axes
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize(); // Normalize to avoid faster diagonal movement

        // Flip sprite based on horizontal movement direction
        if (movement.x != 0)
            spriteRenderer.flipX = movement.x < 0;

        // Check for death condition
        if (!isDead && playerHealth <= 0)
        {
            isDead = true;
            OnPlayerDeath?.Invoke(); // Notify subscribers
            Die();
            Debug.Log("Player died!");
        }
    }

    /// <summary>
    /// Reduces player health by damage amount and plays damage sound.
    /// Ignores damage if player is already dead.
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        AudioManager.Instance.PlaySFX(AudioManager.Instance.DamageSound);
        playerHealth -= damage;
        Debug.Log($"Player took {damage} damage. Current health: {playerHealth}");
    }

    /// <summary>
    /// Increments enemy kill count and notifies stats changed.
    /// </summary>
    public void AddKill()
    {
        enemiesDefeated++;
        OnStatsChanged?.Invoke();
    }

    /// <summary>
    /// Adds amount to damage dealt and notifies stats changed.
    /// </summary>
    public void AddDamage(int amount)
    {
        damageDealt += amount;
        OnStatsChanged?.Invoke();
    }

    private void FixedUpdate()
    {
        // Move the player based on input and speed, using physics engine
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;

        // Calculate world space boundaries based on camera viewport
        Vector2 min = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));

        // Clamp player position so it stays within screen bounds
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }

    /// <summary>
    /// Handles player death: shows game over UI and disables player game object.
    /// </summary>
    private void Die()
    {
        Debug.Log("Player has died!");
        FindFirstObjectByType<GameOverUI>().ShowGameOver();
        gameObject.SetActive(false);
    }
}
