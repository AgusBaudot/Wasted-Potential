using NUnit.Framework;
using UnityEngine.TextCore.Text;

[TestFixture]
public class HealthTests
{
    [Test]
    public void Constructor_SetsMaxAndCurrent()
    {
        //Arrange & Act
        var health = new Health(100);
        
        //Assert
        Assert.AreEqual(100, health.Max);
        Assert.AreEqual(100, health.Current);
    }

    [Test]
    public void TakeDamage_DecreasesHealth()
    {
        //Arrange
        var health = new Health(100);
        
        //Act
        health.TakeDamage(30);
        
        //Assert
        Assert.AreEqual(70, health.Current);
    }

    [Test]
    public void TakeDamage_CannotGoNegative()
    {
        //Arrange
        var health = new Health(100);
        
        //Act
        health.TakeDamage(150);
        
        //Assert
        Assert.AreEqual(0, health.Current);
    }

    [Test]
    public void TakeDamage_FiresOnHealthChangedEvent()
    {
        //Arrange
        var health = new Health(100);
        bool eventFired = false;
        health.OnHealthChanged += (current, max) => eventFired = true;
        
        //Act
        health.TakeDamage(10);
        
        Assert.IsTrue(eventFired);
    }

    [Test]
    public void TakeDamage_ToZero_FiresOnDeathEvent()
    {
        //Arrange
        var health = new Health(100);
        bool deathFired = false;
        health.OnDeath += () => deathFired = true;
        
        //Act
        health.TakeDamage(100);
        
        //Assert
        Assert.IsTrue(deathFired);
    }
}
