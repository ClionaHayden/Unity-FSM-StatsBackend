using UnityEngine;

/// <summary>
/// Interface for all states in the enemy state machine.
/// Defines the contract that each state must follow.
/// </summary>
public interface IState
{
    /// <summary>
    /// Called when entering the state.
    /// Used for initialization or triggering entry actions.
    /// </summary>
    void Enter();

    /// <summary>
    /// Called every frame while in the state.
    /// Used for logic such as movement, detection, or decision-making.
    /// </summary>
    void Update();

    /// <summary>
    /// Called when exiting the state.
    /// Used for cleanup or stopping actions.
    /// </summary>
    void Exit();
}
