using UnityEditor;
using UnityEngine;

public class MenuState : IState
{
    private readonly IGameStateController _controller;

    public MenuState(IGameStateController controller)
    {
        _controller = controller;
    }

    public void EnterState()
    {
        Debug.Log("Entering Main Menu State");
        // Show main menu UI, stop gameplay, etc.
    }

    public void HandleState()
    {
        // Example: if player presses Start, transition to Playing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //_controller.ChangeState(new PlayingState(_controller));
        }
    }

    public void ExitState()
    {
        Debug.Log("Exiting Main Menu State");
        // Hide menu UI, cleanup, etc.
    }
}
