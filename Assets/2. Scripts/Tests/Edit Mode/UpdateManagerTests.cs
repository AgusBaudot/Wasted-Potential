using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class UpdateManagerTests
{
    private IUpdateManager _updateManager;
    private GameObject _go;

    [SetUp]
    public void SetUp()
    {
        _go = new GameObject("UpdateManager");
        _updateManager = _go.AddComponent<UpdateManager>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_go);
    }

    [Test]
    public void Register_AddsUpdatable()
    {
        //Arrange
        var mockUpdatable = new MockUpdatable();
        
        //Act
        _updateManager.Register(mockUpdatable);
        //Trigger Unity's update (call manually or use Unity Test Framework
        
        //Assert
        Assert.IsTrue(mockUpdatable.WasUpdated);
    }

    [Test]
    public void Unregister_RemovesUpdatable()
    {
        //Arrange
        var mockUpdatable = new MockUpdatable();
        _updateManager.Register(mockUpdatable);
        
        //Act
        _updateManager.Unregister(mockUpdatable);
        
        //Assert
        //Verify it's no longer updated.
    }

    private class MockUpdatable : IUpdatable
    {
        public bool WasUpdated {get; private set; }

        public void Tick(float deltaTime) => WasUpdated = true;
    }
}