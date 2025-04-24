using Fusion;
using UnityEngine;
public struct NetInput : INetworkInput
{
    public Vector2 MovementDirection;
    public Vector2 RotationDelta;
}


public struct NetInput_XR : INetworkInput
{
    public Vector2 MoveAxis;
    public Vector2 RotationAxis;
    public float GripAxis;
    public float TriggerAxis;
    public bool GripButton;
    public bool TriggerButton;
}