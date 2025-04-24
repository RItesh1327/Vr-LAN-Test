using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System;




#if UNITY_EDITOR
using UnityEditor;
#endif
public class InputReader : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;
    public InputActionAsset InputActions => inputActions;

    private InputAction moveAction;
    private InputAction lookAction;
    struct InputActionData
    {
        public Vector2 movement;
        public Vector2 look;
    }

    InputActionData inputActionData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!inputActions.enabled)
        {
            inputActions.Enable();
        }
        

        BindAction("MoveAction", ctx =>  inputActionData.movement = ctx.ReadValue<Vector2>(), ctx => inputActionData.movement = Vector2.zero);
        BindAction("LookAction", ctx =>  inputActionData.look = ctx.ReadValue<Vector2>(), ctx => inputActionData.look = Vector2.zero);

    }

    private void BindAction(string actionName, Action<InputAction.CallbackContext> callback_performed, Action<InputAction.CallbackContext> callback_Cancled)
    {
        var action = inputActions.FindAction(actionName);
        if (action != null)
        {
            action.performed += callback_performed;
            action.canceled += callback_Cancled;
        }
        else
            Debug.LogError($"Input Action '{actionName}' not found!");
    }

    private void Update()
    {
        EventBus<PlayerInput>.Raise(new PlayerInput
        {
            movement = inputActionData.movement,
            look = inputActionData.look
        });
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(InputReader))]
public class InputReaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        InputReader inputReader = (InputReader)target;
        GUILayout.Label(Gamepad.all.Count > 0 ? "Gamepad connected" : "No gamepad connected");
        GUILayout.Label(inputReader.InputActions.enabled ? "Inputs enabled" : "Inputs not enabled");
        //GUILayout.Label(inputReader.InputActions.bindingMask.Value.groups);
    }
}
#endif