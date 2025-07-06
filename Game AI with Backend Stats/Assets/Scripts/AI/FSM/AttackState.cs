using UnityEngine;

/// <summary>
/// Represents the enemy's attack behavior in a state machine.
/// </summary>
public class AttackState : IState
{
    private EnemyAI enemy;                  // Reference to the enemy AI script
    private StateMachine stateMachine;      // Reference to the state machine controlling states

    public ChaseState ChaseState { get; set; } // Reference to the chase state, used for transitioning back if player moves away

    private float attackCooldown = 2f;      // Time between attacks
    private float lastAttackTime;           // Last time the enemy attacked

    /// <summary>
    /// Constructor to initialize references.
    /// </summary>
    public AttackState(EnemyAI enemy, StateMachine sm)
    {
        this.enemy = enemy;
        this.stateMachine = sm;
    }

    /// <summary>
    /// Called when entering the attack state.
    /// </summary>
    public void Enter()
    {
        Debug.Log("Entering Attack");
        lastAttackTime = -attackCooldown; // Allow immediate attack on enter
    }

    /// <summary>
    /// Called every frame while in this state.
    /// Handles attacking behavior and transitions.
    /// </summary>
    public void Update()
    {
        // Make the enemy face the player
        Vector3 scale = enemy.transform.localScale;
        if (enemy.player.position.x < enemy.transform.position.x)
        {
            scale.x = -Mathf.Abs(scale.x); // Face left
        }
        else
        {
            scale.x = Mathf.Abs(scale.x);  // Face right
        }
        enemy.transform.localScale = scale;

        // Check distance to player
        float distance = Vector3.Distance(enemy.transform.position, enemy.player.position);

        // If player is out of attack range, switch to chase state
        if (distance > enemy.attackRange)
        {
            stateMachine.SetState(ChaseState);
            return;
        }

        // If cooldown has passed, perform attack
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    /// <summary>
    /// Called when exiting the attack state.
    /// </summary>
    public void Exit()
    {
        Debug.Log("Exiting Attack");
    }

    /// <summary>
    /// Performs the attack logic.
    /// </summary>
    private void Attack()
    {
        Debug.Log("Enemy attacks player!");
        FireSpell();
    }

    /// <summary>
    /// Spawns and initializes the spell projectile.
    /// </summary>
    void FireSpell()
    {
        // Determine spawn position slightly in front of enemy based on facing direction
        Vector3 spawnPos = enemy.transform.position;
        float facingDir = Mathf.Sign(enemy.transform.localScale.x);
        float spawnDistance = 1f; 
        spawnPos += new Vector3(spawnDistance * facingDir, 0, 0);

        // Instantiate the projectile prefab at the calculated position
        GameObject spellGO = Object.Instantiate(enemy.spellPrefab, spawnPos, Quaternion.identity);
    
        // Initialize the spell with the target (player's position)
        SpellProjectile spell = spellGO.GetComponent<SpellProjectile>();
        if (spell != null)
        {
            spell.Initialize(enemy.player.position);
        }
    }
}
