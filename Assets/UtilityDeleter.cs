using UnityEngine;
using Autohand;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class UtilityDeleter : MonoBehaviour
{
    public void RemoveUnUsedUtilities()
    {
        var k = GetComponent<Hand>();
        DestroyImmediate(k);
        var j = GetComponent<HandFollow>();
        DestroyImmediate(j);
        var i = GetComponent<HandAnimator>();
        DestroyImmediate(i);
        var h = GetComponent<HandGrabbableHighlighter>();
        DestroyImmediate(h);
        DestroyImmediate(this);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(UtilityDeleter))]
public class UtilityDeleterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Delete Utility Deleter"))
        {
            var deleter = (UtilityDeleter)target;
            deleter.RemoveUnUsedUtilities();
        }
    }
}
#endif
