using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Targetable : MonoBehaviour
{
    public static Targetable CurrentTarget;

    private float OutlineWidth = 5.0f;

    Outline outline;

    new private Collider collider;

    protected virtual void Start()
    {
        outline = gameObject.AddComponent<Outline>();
        collider = GetComponent<Collider>();

        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = GetOutlineColor();
        outline.OutlineWidth = OutlineWidth;
        outline.enabled = false;
    }

    public void UnTarget()
    {
        outline.enabled = false;
    }

    public void Target()
    {
        outline.enabled = true;
    }

    private void OnMouseOver()
    {
        if (CurrentTarget != null)
        {
            CurrentTarget.UnTarget();
        }

        CurrentTarget = this;
        Target();
    }

    private void OnMouseExit()
    {
        if (CurrentTarget == this)
        {
            UnTarget();
            CurrentTarget = null;
        }
    }

    public float GetDistanceTo(Vector3 source)
    {
        var closestPoint = collider.ClosestPoint(source);
        return Vector3.Distance(closestPoint, source);
    }

    protected abstract Color GetOutlineColor();

    public abstract bool IsAttackable();
}

    
