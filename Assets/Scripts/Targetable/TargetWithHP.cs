using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetWithHP : Hittable
{
    [SerializeField]
    private Resource HP = null;

    protected override void Start()
    {
        base.Start();
    }

    public override void TakeHit(float damage, Enums.Element damageType)
    {
        if (HP.GetValue() != HP.minValue && HP.DecreaseValue(damage) == HP.minValue) //TODO: Enemy controller with resistances through scriptable object
        {
            GetComponent<Collider>().enabled = false;
            GetComponent<Animator>().SetBool("Death", true);
        }
    }
}
