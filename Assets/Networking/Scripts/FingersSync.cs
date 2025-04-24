using Fusion;
using System;
using UnityEngine;

public class FingersSync : NetworkBehaviour
{
    [SerializeField] Transform[] m_FIngerJoints;
    [SerializeField] bool isValid;
    [Networked, Capacity(40)] private NetworkArray<Quaternion> syncedRotations => default;
    public override void Spawned()
    {
        Debug.Log(HasInputAuthority ? "Spawnned on host" : "Spawnned on client");
        isValid = Object.IsInSimulation;
    }
    public override void FixedUpdateNetwork()
    {
        if (HasInputAuthority)
        {
            for (int i = 0; i < m_FIngerJoints.Length; i++)
            {
                syncedRotations.Set(i, m_FIngerJoints[i].rotation);
            }
            Debug.Log("Setting finger rotation on local player");
        }
    }

    public override void Render()
    {
        if(!HasInputAuthority)
        {
            for (int i = 0; i < m_FIngerJoints.Length; i++)
            {
                m_FIngerJoints[i].rotation = syncedRotations.Get(i);
            }
            Debug.Log("Setting rotations on client side");
        }
    }
}
