using Fusion;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms;
using UnityEngine.XR;

public class PlayerSpawner : NetworkBehaviour
{
    [Networked] public bool IsVR { get; private set; } // Sync VR state across network
    [SerializeField] GameObject VRAvatar;
    [SerializeField] GameObject VRNetworkRig;
 
    public override void Spawned()
    {
        bool isLocal = Object.HasInputAuthority;
        bool isHost = Runner.IsServer && Object.HasStateAuthority;

        bool isRemotePlayer = !isLocal;
        if (isHost)
        {
            setSpawnPosition(new Vector3(-0.33f, 0.0f, -4.321f)); // Host position
        }
        else if (isLocal && !isHost)
        {
            setSpawnPosition(new Vector3(3.21f, 0f, -8.33f)); // Client local player
        }
        if (Object.HasInputAuthority)  // Only local player determines its VR state
        {
            bool isVR = XRSettings.isDeviceActive;
            //RPC_RequestAvatarSpawn(isVR);
            VRAvatar.SetActive(true);
            VRNetworkRig.SetActive(true);
            // locally disable all the meshrenders in the VrNetworkRig, recursively find all the objects of type renderer and disable the component
            var meshRenderers = VRNetworkRig.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in meshRenderers)
            {
                renderer.enabled = false;
            }
            Object.RequestStateAuthority();
            if (Object.HasStateAuthority) Debug.Log($"I own this object's state for {gameObject.name}.");
            if (Object.HasInputAuthority) Debug.Log($"I'm the one providing input for {gameObject.name}");
        }
        else
        {
            // If not local player, set the avatar based on the host's VR state
            VRAvatar.SetActive(false);
            VRNetworkRig.SetActive(true);
        }
    }

    void setSpawnPosition(Vector3 position)
    {
        transform.position = position;
    }
}
