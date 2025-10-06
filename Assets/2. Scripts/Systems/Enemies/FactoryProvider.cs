using System;
using UnityEngine;

public class FactoryProvider : MonoBehaviour
{
    public BearFactory bearFactory;
    public RabbitFactory rabbitFactory;
    
    public IEnemyFactory GetFactory(EnemyType type)
    {
        return type switch
        {
            EnemyType.Bear => bearFactory,
            EnemyType.Rabbit => rabbitFactory,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

public enum EnemyType
{
    Bear,
    Rabbit
}
