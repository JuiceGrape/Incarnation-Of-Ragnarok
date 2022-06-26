using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackHandler : MonoBehaviour
{
    private IAttackProvider source = null;
    private ProjectileSource projectileSource = null;
    // Start is called before the first frame update
    private IDamageTaker target = null;
    void Start()
    {
        source = GetComponent<IAttackProvider>();
        if (source == null)
        {
            Debug.LogError("No attack provider attached to " + transform.name);
        }

        projectileSource = GetComponentInChildren<ProjectileSource>();
        if (projectileSource == null)
        {
            Debug.LogError("No projectile source attached to " + transform.name);
        }
    }

    public void SetTarget(IDamageTaker target)
    {
        this.target = target;
    }


    public void AttackTriggered() //Called by animation
    {
        var projectile = source.GetProjectile();
        if (projectile)
        {
            if (!projectileSource)
            {
                Debug.LogError("Tried to spawn a projectile but did not have a source location");
                return;
            }
            var spawnedProjectile = GameObject.Instantiate<Projectile>(projectile, projectileSource.transform.position, transform.rotation);
            spawnedProjectile.Target(target); // If no target, this returns null and is handled in the firing code
            spawnedProjectile.Initialize(source.GetAttackRange() * 1.1f, source.GetDamage(), source.GetDamageType());
        }
        else if (target != null)
        {
            target.TakeHit(source.GetDamage(), source.GetDamageType());
        }
    }
}
