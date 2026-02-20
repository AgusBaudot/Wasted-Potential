using System;

public interface IWaveQuery
{
    event Action<int> OnWaveStarted; //Wave index
    event Action<int> OnWaveCompleted;
    event Action AllWavesCompleted;
    event Action OnNewCardOffer;

    void StartWaves();
    void ConfirmNextWave();
    void StopWaves();
}