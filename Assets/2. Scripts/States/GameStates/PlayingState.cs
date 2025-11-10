using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayingState : IState
{
    private readonly IGameStateController _controller;
    private ISubState _currentSubState;

    public PlayingState(int level)
    {
        _controller = ServiceLocator.Get<IGameStateController>();
        SceneManager.LoadScene($"Level {level}");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Called after awake/start of scene objects
        SceneManager.sceneLoaded -= OnSceneLoaded;
        
        SetSubState(new ShowCards());
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
