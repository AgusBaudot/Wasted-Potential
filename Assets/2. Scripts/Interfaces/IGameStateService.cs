using System;

public interface IGameStateService
{
    IState CurrentState { get; }
    event Action<IState> OnStateChanged;
}