using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameStarter : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkRunner m_NetworkRunnerPrefab;
    private NetworkRunner networkRunner;
    private bool roomExists = false;

    [SerializeField] NetworkPrefabRef m_PlayerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    public delegate void RoomCheckCallback(bool exists);
    public event RoomCheckCallback OnRoomCheckCompleted;

    #region Photon Implementation
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
        // In Shared mode, whoever has State Authority should do the spawning.
        if (_spawnedCharacters.ContainsKey(player)) return;

        // Only spawn if this instance has authority to do so.
        if (runner.IsSharedModeMasterClient || runner.LocalPlayer == player)
        {
            Debug.Log($"Spawning player for {player}");
            Vector3 spawnPosition = new Vector3(0, 1, 0); // Adjust position as needed

            NetworkObject playerObject = runner.Spawn(m_PlayerPrefab, spawnPosition, Quaternion.identity, player);
            _spawnedCharacters.Add(player, playerObject);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (!networkRunner.IsServer)
            return;
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject playerObject))
        {
            _spawnedCharacters.Remove(player);
            runner.Despawn(playerObject);
        }
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
    #endregion

    void Start()
    {
        if (networkRunner == null)
        {
            networkRunner = Instantiate(m_NetworkRunnerPrefab);
            networkRunner.AddCallbacks(this);
        }
    }

    // 🔹 Called when the player clicks "Connect"
    public async void StartGame(bool isHost)
    {
        if (networkRunner == null)
            networkRunner = Instantiate(m_NetworkRunnerPrefab);

        networkRunner.ProvideInput = true;

        // Generate a random session name
        string sessionName = "TestingSession";
        GameMode mode = isHost ? GameMode.Shared : GameMode.Client;
        var scene = SceneRef.FromIndex(1);
        Debug.Log($"Starting game as {mode} with session: {sessionName}");
        PlayerPrefs.SetString("Authority", isHost ? "Host" : "Client");
        var result = await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = sessionName,
            Scene = scene,
            SceneManager = networkRunner.GetComponent<NetworkSceneManagerDefault>(),
        });

        if (result.Ok)
        {
            Debug.Log($"Connected as {mode}");
        }
        else
        {
            Debug.LogError($"Failed to start game: {result.ShutdownReason}");
        }
    }
}
