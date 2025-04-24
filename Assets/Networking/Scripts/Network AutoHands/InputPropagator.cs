using Autohand;
using Fusion;
using UnityEngine;

public class InputPropagator : NetworkBehaviour
{
    AutoHandPlayer player;
    private void Start()
    {
        player = GetComponent<AutoHandPlayer>();
    }
    public override void FixedUpdateNetwork()
    {
        if (!Object.HasInputAuthority)
            return;
        if(GetInput(out NetInput_XR value))
        {
            Debug.LogError($"Move axis value {value.MoveAxis}, Rotation axis Value {value.RotationAxis}");
            player.Move(value.MoveAxis);
            player.Turn(value.RotationAxis.x);
        }
    }
}
