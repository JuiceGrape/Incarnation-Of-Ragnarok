using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : Targetable
{
    public override bool IsAttackable()
    {
        return false;
    }

    public virtual void Interact()
    {
        Debug.Log("Player interacted with " + transform.name);
    }

    protected override Color GetOutlineColor()
    {
        return Color.green;
    }
}
