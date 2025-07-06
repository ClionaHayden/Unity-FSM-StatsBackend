using UnityEngine;

/// <summary>
/// Represents the patrol behavior in the enemy's state machine.
/// </summary>
public class PatrolState : IState
{
    private EnemyAI enemy;                 // Reference to the EnemyAI controlling this state
    private StateMachine stateMachine;    // Reference to the overarching state machine

    public ChaseState ChaseState { get; set; } // Reference to the chase state for transitions

    /// <summary>
    /// Constructor for the PatrolState.
    /// </summary>
    public PatrolState(EnemyAI enemy, StateMachine sm)
    {
        this.enemy = enemy;
        this.stateMachine = sm;
    }

    /// <summary>
    /// Called when entering the patrol state.
    /// Useful for starting patrol animations or resetting patrol points.
    /// </summary>
    public void Enter()
    {
        Debug.Log("Entering Patrol");
    }

    /// <summary>
    /// Called every frame during the patrol state.
    /// Currently checks distance to player and transitions to chase if player is close.
    /// </summary>
    public void Update()
    {
        // Check if player is within chase range
        float dist = Vector3.Distance(enemy.transform.position, enemy.player.position);
        if (dist < enemy.chaseRange)
        {
            stateMachine.SetState(ChaseState); // Switch to chasing the player
        }
    }

    /// <summary>
    /// Called when exiting the patrol state.
    /// Useful for stopping animations or resetting state.
    /// </summary>
    public void Exit()
    {
        Debug.Log("Exiting Patrol");
    }
}
