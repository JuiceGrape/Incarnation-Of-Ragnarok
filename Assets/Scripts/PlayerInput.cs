using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    public float SecondsBeforeMouseHold = 0.1f;

    public UnityEvent<Vector3> OnMousePosChanged;
    public UnityEvent<Enums.InputState> OnMovementChanged;

    private Vector3 MousePos;

    private Enums.InputState MouseState;
    private float SecondsSinceMouseDown = 0.0f;

    // Update is called once per frame
    void Update()
    {   
        if (MousePos == null || Input.mousePosition != MousePos)
        {
            MousePos = Input.mousePosition;
            OnMousePosChanged.Invoke(MousePos);
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
