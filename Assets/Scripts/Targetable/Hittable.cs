using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : Targetable
{
    ProjectileLocation projectileTarget;
    protected override void Start()
    {
        base.Start();
        projectileTarget = GetComponentInChildren<ProjectileLocation>();
        if (!projectileTarget)
            Debug.LogError("Hittable has no projectile target. Defaulting to own location as target"); //TODO: Maybe warning instead?
        else
            Debug.Log("TEST");

    }
    public Vector3 GetHitPosition()
    {
        if (projectileTarget)
            return projectileTarget.transform.position;
        else
            return transform.position;
    }

    public override bool IsAttackable()
    {
        return true; //TODO: Based on HP, maybe?
    }

    public virtual void TakeHit(float damage, Enums.Element damageType)
    {
        Debug.Log(transform.name + " took " + damage + " " + damageType + " damage"); 
    }

    protected override Color GetOutlineColor()
    {
        return Color.red;
    }
}
