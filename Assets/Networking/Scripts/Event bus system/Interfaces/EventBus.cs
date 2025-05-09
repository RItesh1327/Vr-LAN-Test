using System;
using System.Collections.Generic;
using System.Diagnostics;

public interface IEvent { }

public static class EventBus<T> where T : IEvent 
{
    static readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();
    public static void Register(EventBinding<T> binding) { bindings.Add(binding); }
    public static void DeRegister(EventBinding<T> binding) { bindings.Remove(binding); }
    public static void Raise(T @event)
    {
        foreach(var binding in bindings)
        {
            binding.OnEvent.Invoke(@event);
            binding.OnEventNoArgs.Invoke();
        }
    }
    static void Clear()
    {
        UnityEngine.Debug.Log($"Clearing {typeof(T).Name} bindings");
        bindings.Clear();
    }
}
