using Fusion;
using UnityEngine;

public class AttachToMainCamera : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (HasInputAuthority)
        {
            FindAnyObjectByType<CameraFollowTarget>().SetFollowTargetTransform(transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
