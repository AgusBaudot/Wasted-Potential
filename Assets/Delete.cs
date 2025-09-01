using UnityEngine;

public class Delete : MonoBehaviour
{
    public WaveManager wave;

    private void Start()
    {
        wave.StartWaves();
    }
}