using Autohand;
using UnityEngine;

public class FingerTester : MonoBehaviour
{
    [SerializeField][Range(0, 1)] float bendOffset;
    Finger cachedFinger;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cachedFinger = GetComponent<Finger>();
    }

    // Update is called once per frame
    void Update()
    {
        cachedFinger.UpdateFingerPose(bendOffset);
    }
}
