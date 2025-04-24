using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
public static class EventBusUtilities
{
    public static IReadOnlyList<Type> EventTypes { get; set; }
    public static IReadOnlyList<Type> EventBusTypes { get; set; }

#if UNITY_EDITOR
    public static PlayModeStateChange PlayModeStateChange { get; set; }
    [InitializeOnLoadMethod]
    public static void InitializeEditor()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }
    static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        PlayModeStateChange = state;
        if(state == PlayModeStateChange.ExitingPlayMode)
        {
            ClearAllBusses();
        }
    }
#endif

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        EventTypes = PreDefinedAssemblyUtility.GetTypes(typeof(IEvent));
        EventBusTypes = initializeAllBusses();
    }

    private static IReadOnlyList<Type> initializeAllBusses()
    {
        List<Type> eventBusTypes = new List<Type>();
        var typedef = typeof(EventBus<>);

        foreach (var type in EventTypes)
        {
            var busType = typedef.MakeGenericType(type);
            eventBusTypes.Add(busType);
            Debug.Log($"Initialized EventBus<{type.Name}");
        }
        return eventBusTypes;
    }

    public static void ClearAllBusses()
    {
        Debug.Log("Clearning all busses");
        for (int i = 0;i<EventBusTypes.Count;i++)
        {
            var busType = EventBusTypes[i];
            var clearMethod = busType.GetMethod("Clear", BindingFlags.Static | BindingFlags.NonPublic);
            clearMethod.Invoke(null, null);
        }
    }
}
