using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Handles the enemy AI behavior using a state machine (Patrol, Chase, Attack).
/// Also manages health, death, and combat interactions.
/// </summary>
public class EnemyAI : MonoBehaviour
{
    // State machine that controls current behavior
    private StateMachine stateMachine;

    // Reference to the player and their controller
    private PlayerController playerController;
    public Transform player;

    // Ranges for AI behavior switching
    public float chaseRange = 5f;
    public float attackRange = 1.5f;

    // Health management
    public int maxHealth = 3;
    private int currentHealth;

    // Event triggered when the enemy dies (for scorekeeping or other logic)
    public event System.Action OnEnemyDeath;

    // Spell attack
    public GameObject spellPrefab; // Assign spell prefab in the Inspector
    public Transform firePoint;    // Where spells spawn from

    // Health bar UI (world space)
    private EnemyHealthBar healthBar;

    /// <summary>
    /// Initializes state machine and references.
    /// </summary>
    private void Start()
    {
        // Play spawn sound
        AudioManager.Instance.PlaySFX(AudioManager.Instance.enemySpawnSound);

        // Set initial health
        currentHealth = maxHealth;
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        healthBar.SetHealth(currentHealth);

        // Initialize the state machine and states
        stateMachine = new StateMachine();

        var patrol = new PatrolState(this, stateMachine);
        var chase = new ChaseState(this, stateMachine);
        var attack = new AttackState(this, stateMachine);

        // Link states to allow transitions
        patrol.ChaseState = chase;
        chase.PatrolState = patrol;
        chase.AttackState = attack;
        attack.ChaseState = chase;

        // Begin in patrol state
        stateMachine.SetState(patrol);

        // Find the player in the scene using tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerController = playerObj.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogError("Player not found! Make sure your player is tagged as 'Player'.");
        }
    }

    /// <summary>
    /// Updates the current state logic.
    /// </summary>
    private void Update()
    {
        stateMachine.Update();
    }

    /// <summary>
    /// Handles receiving damage, updating health and death if necessary.
    /// </summary>
    /// <param name="amount">The amount of damage to take.</param>
    public void TakeDamage(int amount)
    {
        // Play damage sound
        AudioManager.Instance.PlaySFX(AudioManager.Instance.DamageSound);

        // Subtract health
        currentHealth -= amount;

        // Update health bar
        float normHealth = Mathf.Clamp01((float)currentHealth / maxHealth);
        healthBar?.SetHealth(normHealth);

        Debug.Log($"{gameObject.name} took {amount} damage. Remaining: {currentHealth}");

        // Notify player controller (e.g., for damage dealt stat)
        if (playerController != null)
        {
            playerController.AddDamage(amount);
        }

        // If health reaches zero, die
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Handles enemy death: triggers events, updates player stats, and destroys the object.
    /// </summary>
    private void Die()
    {
        Debug.Log($"{gameObject.name} died.");

        // Notify player controller of kill
        if (playerController != null)
        {
            playerController.AddKill();
        }

        // Trigger external death event
        OnEnemyDeath?.Invoke();

        // Destroy the enemy GameObject
        Destroy(gameObject);
    }
}
