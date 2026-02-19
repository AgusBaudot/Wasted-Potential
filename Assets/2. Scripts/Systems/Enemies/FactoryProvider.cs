using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class FactoryProvider : MonoBehaviour
{
    [SerializeField] private BearFactory bearFactory;
    [SerializeField] private RabbitFactory rabbitFactory;
    [SerializeField] private DeerFactory deerFactory;
    [SerializeField] private BeaverFactory beaverFactory;
    [SerializeField] private ForestBeastFactory forestBeastFactory;

    [Inject]
    public void Construct(IObjectResolver container)
    {
        container.InjectGameObject(beaverFactory.gameObject);
        container.InjectGameObject(rabbitFactory.gameObject);
        container.InjectGameObject(bearFactory.gameObject);
        container.InjectGameObject(deerFactory.gameObject);
        container.InjectGameObject(forestBeastFactory.gameObject);
    }

    public IEnemyFactory GetFactory(EnemyType type)
    {
        return type switch
        {
            EnemyType.Bear => bearFactory,
            EnemyType.Rabbit => rabbitFactory,
            EnemyType.Deer => deerFactory,
            EnemyType.Beaver => beaverFactory,
            EnemyType.Boss => forestBeastFactory,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
