using UnityEditor;
using UnityEngine;

public class PlayingState : IState
{
    private readonly IGameStateController _controller;

    public PlayingState(IGameStateController controller)
    {
        _controller = controller;
    }

    public void EnterState()
    {
        Debug.Log("Entering Playing State");
        // Enable gameplay systems, spawn player, etc.
    }

    public void HandleState()
    {
        // Example: pause game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //_controller.ChangeState(new PausedState(_controller));
        }
    }

    public void ExitState()
    {
        Debug.Log("Exiting Playing State");
        // Disable gameplay systems if necessary
    }
}
