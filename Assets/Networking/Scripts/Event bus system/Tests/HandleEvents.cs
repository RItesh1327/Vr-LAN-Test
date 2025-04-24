using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleEvents : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        EventBus<TestEvent>.Raise(new TestEvent());
        EventBus<TestEventArgs>.Raise(new TestEventArgs
        {
            health = 100,
            dammage = 150,
        });
    }
}
