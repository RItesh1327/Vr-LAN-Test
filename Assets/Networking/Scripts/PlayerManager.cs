using Fusion;
using UnityEngine;
using static Unity.Collections.Unicode;

public class PlayerManager : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] NetworkPrefabRef m_PlayerPrefab;
    private NetworkDictionary<PlayerRef, NetworkObject> Players = new NetworkDictionary<PlayerRef, NetworkObject>();
    public async void PlayerJoined(PlayerRef player)
    {
        if(HasStateAuthority)
        {
            Debug.Log($"Spawning player for {player}");
            await Runner.LoadScene("Demo");
            NetworkObject playerObject = Runner.Spawn(m_PlayerPrefab, new Vector3(-0.033f, 0f, -4.321f), Quaternion.identity, player);
            Players.Add(player, playerObject);
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (!HasStateAuthority)
            return;
        if(Players.TryGet(player, out NetworkObject playerObject))
        {
            Players.Remove(player);
            Runner.Despawn(playerObject);
        }
       
    }
}
