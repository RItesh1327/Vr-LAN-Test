using System;
using UnityEngine;
public struct TestEvent : IEvent { }
public struct TestEventArgs : IEvent
{
    public int health;
    public int dammage;
}



public struct PlayerInput : IEvent
{
    public Vector2 movement;
    public Vector2 look;
}