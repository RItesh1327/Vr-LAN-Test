using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class LanNetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkRunner runnerPrefab;
    private NetworkRunner runnerInstance;

    [Header("Set your scene from build index here")]
    public SceneRef networkScene;

    [SerializeField] NetworkPrefabRef m_PlayerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    public async void StartHost()
    {
        runnerInstance = Instantiate(runnerPrefab);
        runnerInstance.ProvideInput = true;

        // Start host (server) with the specific port
        await runnerInstance.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Server,
            Address = NetAddress.Any(27015), // Bind to all local interfaces on port 27015
            SessionName = "LANGame",
            Scene = networkScene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    // CLIENT
    public async void JoinAsClient()
    {
        runnerInstance = Instantiate(runnerPrefab);
        runnerInstance.ProvideInput = true;

        // Use NetAddress.Parse in Fusion 2.0.5 to specify the host IP
        NetAddress targetAddress = NetAddress.CreateFromIpPort(GetLocalIPAddress(), 27015); 

        // Join the game as a client
        await runnerInstance.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Client,
            Address = targetAddress,  // Set the address to the host's IP and port
            SessionName = "LANGame",
            Scene = networkScene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }


    string GetLocalIPAddress()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return "127.0.0.1";
    }

    // --- INetworkRunnerCallbacks ---

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"Player joined: {player}");
        // Spawn logic here if needed

        Debug.Log($"Spawning player for {player}");
        Vector3 spawnPosition = new Vector3(0, 1, 0); // Adjust position as needed

        NetworkObject playerObject = runner.Spawn(m_PlayerPrefab, spawnPosition, Quaternion.identity, player);
        _spawnedCharacters.Add(player, playerObject);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.LogError($"Connection failed: {reason}");
    }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, System.ArraySegment<byte> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        throw new NotImplementedException();
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        throw new NotImplementedException();
    }
}
