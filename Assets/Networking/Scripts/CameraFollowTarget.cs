using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    Transform followTarget;
    public void SetFollowTargetTransform(Transform followTarget)
    {
        this.followTarget = followTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if(followTarget == null) return;
        transform.position = followTarget.position;
        transform.rotation = followTarget.rotation;
    }
}
