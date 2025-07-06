using UnityEngine;

/// <summary>
/// Represents the enemy's chasing behavior in the state machine.
/// </summary>
public class ChaseState : IState
{
    private EnemyAI enemy;                 // Reference to the EnemyAI component
    private StateMachine stateMachine;    // Reference to the state machine

    public PatrolState PatrolState { get; set; }  // Reference to patrol state for transitions
    public AttackState AttackState { get; set; }  // Reference to attack state for transitions

    /// <summary>
    /// Constructor for the chase state.
    /// </summary>
    public ChaseState(EnemyAI enemy, StateMachine sm)
    {
        this.enemy = enemy;
        this.stateMachine = sm;
    }

    /// <summary>
    /// Called when entering the chase state.
    /// </summary>
    public void Enter()
    {
        Debug.Log("Entering Chase");
    }

    /// <summary>
    /// Called every frame while in the chase state.
    /// Handles movement, facing, and state transitions.
    /// </summary>
    public void Update()
    {
        // Face the player
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

        // Measure distance to the player
        float distance = Vector3.Distance(enemy.transform.position, enemy.player.position);

        // If player is too far, return to patrol
        if (distance > enemy.chaseRange)
        {
            stateMachine.SetState(PatrolState);
            return;
        }

        // If player is within attack range, transition to attack state
        if (distance <= enemy.attackRange)
        {
            stateMachine.SetState(AttackState);
            return;
        }

        // Move toward the player
        Vector3 dir = (enemy.player.position - enemy.transform.position).normalized;
        enemy.transform.position += dir * Time.deltaTime * 2f; // 2f = chase speed
    }

    /// <summary>
    /// Called when exiting the chase state.
    /// </summary>
    public void Exit()
    {
        Debug.Log("Exiting Chase");
    }
}
