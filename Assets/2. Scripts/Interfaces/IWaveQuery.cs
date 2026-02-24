using System;
using System.Collections.Generic;

public interface IWaveQuery
{
    List<Wave> AllWaves { get; }
    
    event Action<int> OnWaveStarted; //Wave index
    event Action<int> OnWaveCompleted;
    event Action AllWavesCompleted;
    event Action OnNewCardOffer;

    void StartWaves();
    void ConfirmNextWave();
    void StopWaves();
}