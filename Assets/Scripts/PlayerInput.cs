using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    public float SecondsBeforeMouseHold = 0.1f;

    public UnityEvent<Vector3> OnMousePosChanged;
    public UnityEvent<Enums.InputState> OnMovementChanged;

    public UnityEvent<int, Enums.InputState> OnAbilityStateChanged;

    private Vector3 MousePos;

    private Enums.InputState MouseState;
    private float SecondsSinceMouseDown = 0.0f;

    private Dictionary<int, KeyCode> AbilityKeys = new Dictionary<int, KeyCode>()
    {
        {0, KeyCode.Space }, //Maybe fully separate ability, otherwise ability 0 is always dash ability
        {1, KeyCode.Q },
        {2, KeyCode.W },
        {3, KeyCode.E },
        {4, KeyCode.R }
    };

    // Update is called once per frame
    void Update()
    {   
        if (MousePos == null || Input.mousePosition != MousePos)
        {
            MousePos = Input.mousePosition;
            OnMousePosChanged.Invoke(MousePos);
        }

        foreach(var val in AbilityKeys)
        {
            if (Input.GetKeyDown(val.Value))
            {
                OnAbilityStateChanged.Invoke(val.Key, Enums.InputState.Down);
            }
            else if (Input.GetKeyUp(val.Value))
            {
                OnAbilityStateChanged.Invoke(val.Key, Enums.InputState.Up);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            MouseState = Enums.InputState.Down;
            SecondsSinceMouseDown = 0.0f;
            OnMovementChanged.Invoke(MouseState);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            MouseState = Enums.InputState.Up;
            OnMovementChanged.Invoke(MouseState);
        }
        else if (MouseState == Enums.InputState.Down)
        {
            if (SecondsSinceMouseDown < SecondsBeforeMouseHold)
            {
                SecondsSinceMouseDown += Time.deltaTime;
            }
            else
            {
                OnMovementChanged.Invoke(Enums.InputState.Held);
            }
        }

    }
}
