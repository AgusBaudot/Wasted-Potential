using System;
using UnityEngine;

public class FactoryProvider : MonoBehaviour
{
    public BearFactory bearFactory;
    public RabbitFactory rabbitFactory;
    public DeerFactory deerFactory;
    public BeaverFactory beaverFactory;

    public IEnemyFactory GetFactory(EnemyType type)
    {
        return type switch
        {
            EnemyType.Bear => bearFactory,
            EnemyType.Rabbit => rabbitFactory,
            EnemyType.Deer => deerFactory,
            EnemyType.Beaver => beaverFactory,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

public enum EnemyType
{
    Bear,
    Rabbit,
    Deer,
    Beaver
}
