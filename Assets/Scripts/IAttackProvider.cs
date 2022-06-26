using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackProvider
{
    float GetAttackRange();
    float GetAttackDelay();
    float GetDamage();
    Enums.Element GetDamageType();
    Projectile GetProjectile();
}
