using UnityEngine;

public class PlayingState : IState
{
    private readonly IGameStateController _controller = ServiceLocator.Get<IGameStateController>();
    private ISubState _currentSubState;

    public void SetSubState(ISubState newSubState)
    {
        _currentSubState?.Exit();
        _currentSubState = newSubState;
        _currentSubState?.Enter();
    }

    public void EnterState()
    {
        Debug.Log("Entering Playing State");
        // Enable gameplay systems, spawn player, etc.
    }

    public void HandleState()
    {
        _currentSubState?.Update();
    }

    public void ExitState()
    {
        Debug.Log("Exiting Playing State");
        // Disable gameplay systems if necessary
    }
}
