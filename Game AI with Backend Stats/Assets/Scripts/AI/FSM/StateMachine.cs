using UnityEngine;

/// <summary>
/// A simple state machine that handles transitions and updates between states.
/// </summary>
public class StateMachine
{
    // The current active state
    private IState currentState;

    /// <summary>
    /// Sets the new state. Exits the old state and enters the new one.
    /// </summary>
    /// <param name="newState">The state to transition to.</param>
    public void SetState(IState newState)
    {
        // Prevent re-entering the same state
        if (currentState == newState) return;

        // Exit the current state (if any)
        currentState?.Exit();

        // Set and enter the new state
        currentState = newState;
        currentState.Enter();
    }

    /// <summary>
    /// Call this once per frame to update the current state.
    /// </summary>
    public void Update()
    {
        currentState?.Update();
    }

    /// <summary>
    /// Read-only property to access the current state.
    /// </summary>
    public IState CurrentState => currentState;
}
