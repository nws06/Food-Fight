// From https://www.youtube.com/watch?v=maDpJL3go9g
// "STOP Using Singletons! Try THIS Instead – Service Locator in Unity" 
// by Binary Impact on YouTube

using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static Dictionary<Type, object> _services = new();



    public static void Register<T>(T service) where T : class {
        _services[typeof(T)] = service;
    }



    public static T Get<T>() where T : class
    {
        if (_services.TryGetValue(typeof(T), out object service))
            return service as T;
        
        throw new Exception($"ServiceLocator.cs: Service {typeof(T).Name} was not found.");
    }

    public static T TryGet<T>() where T : class
    {
        if (_services.TryGetValue(typeof(T), out object service))
            return service as T;
        
        return null;
    }



    public static void Unregister<T>() where T : class
    {
        _services.Remove(typeof(T));
    }
}
