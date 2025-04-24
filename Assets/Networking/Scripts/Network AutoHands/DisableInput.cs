using Autohand.Demo;
using Fusion;
using UnityEngine;

public class DisableInput : NetworkBehaviour
{
    private void Start()
    {
        if(!HasInputAuthority)
        {
            var k = GetComponents<XRAutoHandFingerBender>();
            var j = GetComponents<XRAutoHandAxisFingerBender>();

            for (int i = 0; i < k.Length; i++)
            {
                k[i].enabled = false;
            }
            for (int i = 0;i < j.Length; i++)
            {
                j[i].enabled = false;
            }
        }
    }
}
