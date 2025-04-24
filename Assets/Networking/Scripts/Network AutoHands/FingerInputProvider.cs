using Autohand;
using UnityEngine;

public class FingerInputProvider : MonoBehaviour
{
    Finger cachedFinger;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cachedFinger = GetComponent<Finger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
