using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkInputManager : SimulationBehaviour, IBeforeUpdate, INetworkRunnerCallbacks, ISpawned
{

    [SerializeField] InputActionAsset inputActions;
    private NetInput accumulatedInput;
    bool resetInput;

    void OnEnable()
    {
        var myNetworkRunner = FindAnyObjectByType<NetworkRunner>();
        if(myNetworkRunner != null)
        {
            myNetworkRunner.AddCallbacks(this);
            Debug.Log("Binding player input");
            if (!inputActions.enabled)
            {
                inputActions.Enable();
                Debug.Log("ENabling INput actions");
            }
            Debug.Log("INput actions enabled");

            BindAction("MoveAction", ctx => inputActionData.movement = ctx.ReadValue<Vector2>(), ctx => inputActionData.movement = Vector2.zero);
            BindAction("LookAction", ctx => inputActionData.look = ctx.ReadValue<Vector2>(), ctx => inputActionData.look = Vector2.zero);

            Debug.Log("Binding to input actions");
        }
    }
    struct InputActionData
    {
        public Vector2 movement;
        public Vector2 look;
    }
    InputActionData inputActionData;

    [SerializeField] Vector2 move;
    [SerializeField] Vector2 look;

    public void BeforeUpdate()
    {
        resetInput = false;
        
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        // Input is actually ready to be used here
        move = inputActionData.movement;
        look = inputActionData.look;
        accumulatedInput.MovementDirection = inputActionData.movement;
        accumulatedInput.RotationDelta = inputActionData.look;
        Debug.Log($"Inputs processed {inputActionData.movement}    {inputActionData.look}");
        input.Set(accumulatedInput);
        resetInput = true;
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if(player == runner.LocalPlayer)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }

    // a good place for using event bus here
    public void Spawned()
    {
        Debug.Log("Player spawnned");
    }

    private void BindAction(string actionName, Action<InputAction.CallbackContext> callback_performed, Action<InputAction.CallbackContext> callback_Cancled)
    {
        var action = inputActions.FindAction(actionName);
        if (action != null)
        {
            action.performed += callback_performed;
            action.canceled += callback_Cancled;
        }
        else
            Debug.LogError($"Input Action '{actionName}' not found!");
    }
}
