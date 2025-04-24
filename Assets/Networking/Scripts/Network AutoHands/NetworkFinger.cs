using Autohand;
using Fusion;
using UnityEngine;

public class NetworkFinger : NetworkBehaviour
{
    Finger cachedFinger;
    [Networked, SerializeField] float bendOffset { get; set; }
    void Start()
    {
        cachedFinger = GetComponent<Finger>();
    }

    public override void FixedUpdateNetwork()
    {
        if(HasInputAuthority)
        {
            if(cachedFinger == null)
            {
                Debug.Log("Cached finger is null");
                return;
            }
            bendOffset = cachedFinger.bendOffset;
            Debug.Log("Setting finger bend");
        }
    }

    public override void Render()
    {
        if(!HasInputAuthority)
        {
            if (cachedFinger == null)
                return;
            cachedFinger.bendOffset = bendOffset;
            Debug.Log("Setting networked finger rotation on local machine");
        }
    }
}
