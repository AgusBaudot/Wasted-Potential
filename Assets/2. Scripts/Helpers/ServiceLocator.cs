using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> _services = new();

    public static void Register<T>(T service)
    {
        var type = typeof(T);
        _services[type] = service;
    }

    public static T Get<T>()
    {
        var type = typeof(T);
        if (_services.TryGetValue(type, out var service))
            return (T)service;

        throw new Exception($"Service of type {type} not found");
    }

    public static void Unregister<T>(T service)
    {
        var type = typeof(T);
        _services.Remove(type);
    }

}