using UnityEngine;
using NUnit.Framework;

[TestFixture]
public class EnemyStatusManagerTests
{
    private StatusEffect _slowEffect;
    private StatusEffect _stunEffect;
    
    [SetUp]
    public void SetUp()
    {
        _slowEffect = ScriptableObject.CreateInstance<StatusEffect>();
        _slowEffect.speedMultiplier = 0.5f;
        
        _stunEffect = ScriptableObject.CreateInstance<StatusEffect>();
        _stunEffect.isStun = true;
    }
    
    [Test]
    public void Apply_AddsStatusToActive()
    {
        var manager = new EnemyStatusManager(mockEnemy);
        
        manager.Apply(_slowEffect);
        
        Assert.IsTrue(manager.Has(_slowEffect));
    }
    
    [Test]
    public void Apply_DifferentEffect_DoesNotConflict()
    {
        var manager = new EnemyStatusManager(mockEnemy);
        
        manager.Apply(_slowEffect);
        manager.Apply(_stunEffect);
        
        Assert.IsTrue(manager.Has(_slowEffect));
        Assert.IsTrue(manager.Has(_stunEffect));
    }
    
    // Add 5-10 more tests
}