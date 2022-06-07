using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    public UnityEvent<Vector3> OnMousePosChanged;
    public UnityEvent<Enums.InputState> OnMovementChanged;

    private Vector3 MousePos;

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
            OnMovementChanged.Invoke(Enums.InputState.Down);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnMovementChanged.Invoke(Enums.InputState.Up);
        }

    }
}
