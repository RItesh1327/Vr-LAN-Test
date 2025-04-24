using Autohand;
using Fusion;
using UnityEngine;

public class SyncPLayerAvatar : NetworkBehaviour
{
    [Header("Local player")]
    [SerializeField] Transform m_PlayerHead;
    [SerializeField] Transform m_Player_LefthHand;
    [SerializeField] Transform m_Player_RightHand;
    [SerializeField] Transform m_LocalPlayer;
    [Header("Remote player")]
    [SerializeField] NetworkTransform m_RemoteHead;
    [SerializeField] NetworkTransform m_Remote_LefthHand;
    [SerializeField] NetworkTransform m_Remote_RightHand;
    [SerializeField] NetworkTransform m_RemotePlayer;

    //[SerializeField] GameObject Hand_Left;
    //[SerializeField] GameObject Hand_Right;
    //[SerializeField] GameObject NetworkHand_Left;
    //[SerializeField] GameObject NetworkHand_Right;
    [Header("Real fingers")]
    [SerializeField]
    Finger[] m_RealFingers;
    [Header("Network fingers")]
    [SerializeField]
    Finger[] m_FakeFingers;
    [Networked, Capacity(10)]
    NetworkArray<float> m_FingerBendOffsets => default;
    //[Networked] Vector3 RightHandPosition { get; set; }
    //[Networked] Quaternion RightHandRotation { get; set; }
    //[Networked] Vector3 LeftHandPosition { get; set; }
    //[Networked] Quaternion LeftHandRotation { get; set; }
    public override void Spawned()
    {

    }
    private void Update()
    {
        if (!HasInputAuthority) return;
        for (int i = 0; i < m_FakeFingers.Length; i++)
        {
            if (m_FakeFingers[i] == null)
            {
                Debug.Log("Cached finger is null");
                return;
            }
            m_FakeFingers[i].SetFingerBend(m_FingerBendOffsets[i]);
        }
        //NetworkHand_Left.transform.SetPositionAndRotation(RightHandPosition, RightHandRotation);
        //NetworkHand_Right.transform.SetPositionAndRotation(LeftHandPosition, LeftHandRotation);
    }
    public override void FixedUpdateNetwork()
    {
        if (!HasInputAuthority) return;
        m_RemoteHead.transform.SetPositionAndRotation(m_PlayerHead.position, m_PlayerHead.rotation);
        m_Remote_LefthHand.transform.SetPositionAndRotation(m_Player_LefthHand.position, m_Player_LefthHand.rotation);
        m_Remote_RightHand.transform.SetPositionAndRotation(m_Player_RightHand.position, m_Player_RightHand.rotation);
        m_RemotePlayer.transform.SetPositionAndRotation(m_LocalPlayer.position, m_LocalPlayer.rotation);
        //RightHandPosition = Hand_Right.transform.position;
        //RightHandRotation = Hand_Right.transform.rotation;
        //LeftHandPosition = Hand_Left.transform.position;
        //LeftHandRotation = Hand_Left.transform.rotation;
        for (int i = 0; i < m_RealFingers.Length; i++)
        {
            if (m_RealFingers[i] == null)
            {
                Debug.Log("Cached finger is null");
                return;
            }
            m_FingerBendOffsets.Set(i, m_RealFingers[i].bendOffset);
        }
    } 
}
