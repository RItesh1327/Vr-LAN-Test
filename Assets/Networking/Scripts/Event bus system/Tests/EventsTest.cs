using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsTest : MonoBehaviour
{
    EventBinding<TestEvent> testEventBinding;
    EventBinding<TestEventArgs> testEventArgsBinding;
    // Start is called before the first frame update
    void Start()
    {
        testEventBinding = new EventBinding<TestEvent>(handleTestEvent);
        EventBus<TestEvent>.Register(testEventBinding);
        testEventArgsBinding = new EventBinding<TestEventArgs>(handleArgEvents);
        EventBus<TestEventArgs>.Register(testEventArgsBinding);
    }
    private void OnDestroy()
    {
        EventBus<TestEvent>.DeRegister(testEventBinding);
        EventBus<TestEventArgs>.DeRegister(testEventArgsBinding);
    }
    private void handleArgEvents(TestEventArgs args)
    {
        Debug.Log(args.health);
        Debug.Log(args.dammage);
    }

    private void handleTestEvent(TestEvent @event)
    {
        Debug.Log("Nothing, just triggered an event");
    }
}
