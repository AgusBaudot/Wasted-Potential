using System;
using UnityEngine;

public class GameStateManager : IGameStateService, IGameStateController, IUpdatable
{
    public IState CurrentState { get; private set; }

    public event Action<IState> OnStateChanged;

    public void ChangeState(IState newState)
    {
        if (newState == CurrentState)
            return;

        CurrentState?.ExitState();
        CurrentState = newState;

        Debug.Log($"[GameStateManager] Changed to state: {newState.GetType().Name}");
        CurrentState.EnterState();

        OnStateChanged?.Invoke(newState);
    }

    public void Tick(float deltaTime)
    {
        CurrentState?.HandleState();
    }
}
