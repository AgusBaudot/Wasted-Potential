using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayingState : IState
{
    private readonly IGameStateController _controller;
    private ISubState _currentSubState;

    public PlayingState(int level)
    {
        _controller = ServiceLocator.Get<IGameStateController>();
        switch (level)
        {
            case 1:
                SceneManager.LoadScene("Level 1");
                break;
            case 2:
                SceneManager.LoadScene("Level 2");
                break;
            case 3:
                SceneManager.LoadScene("Level 3");
                break;
            case 4:
                SceneManager.LoadScene("Level 4");
                break;

            default:
                throw new NotImplementedException($"Level {nameof(level)} not implemented yet.");
        }
    }

    public void SetSubState(ISubState newSubState)
    {
        _currentSubState?.Exit();
        _currentSubState = newSubState;
        _currentSubState?.Enter();
    }

    public void EnterState()
    {
        Debug.Log("Entering Playing State");
        SetSubState(new ShowCards());
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
