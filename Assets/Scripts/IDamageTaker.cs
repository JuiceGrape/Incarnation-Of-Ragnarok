using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageTaker
{
    void TakeHit(float damage, Enums.Element damageType);

    Vector3 GetProjectileTargetPosition();
}
