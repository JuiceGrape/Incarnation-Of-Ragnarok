using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : Targetable, IDamageTaker
{
    ProjectileTarget projectileTarget;
    IndicatorOrigin indicatorOrigin;

    protected override void Start()
    {
        base.Start();
        projectileTarget = GetComponentInChildren<ProjectileTarget>();
        if (!projectileTarget)
            Debug.LogWarning("Hittable has no projectile target. Defaulting to own location as target"); //TODO: Maybe warning instead?

        indicatorOrigin = GetComponentInChildren<IndicatorOrigin>();
        if (!indicatorOrigin)
            Debug.LogWarning("Hittable has no indicator origin. Defaulting to own location as origin"); //TODO: Maybe warning instead?

    }

    public Vector3 GetIndicatorOrigin()
    {
        if (indicatorOrigin)
            return indicatorOrigin.transform.position;
        else
            return transform.position;
    }

    public override bool IsAttackable()
    {
        return true; //TODO: Based on HP, maybe?
    }

    //IDamageTaker
    public virtual void TakeHit(float damage, Enums.Element damageType)
    {
        Debug.Log(transform.name + " took " + damage + " " + damageType + " damage"); 
    }

    public Vector3 GetProjectileTargetPosition()
    {
        if (projectileTarget)
            return projectileTarget.transform.position;
        else
            return transform.position;
    }

    //ETC
    protected override Color GetOutlineColor()
    {
        return Color.red;
    }
}
