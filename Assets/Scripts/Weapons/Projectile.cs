using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float MovementSpeed = 5.0f;

    [SerializeField]
    private bool HitsPlayers = false;

    Vector3 StartPosition;

    IDamageTaker target;

    bool IsFlying = false;

    float totalMovement = 0.0f;

    float damage;
    float range;
    Enums.Element damageType;


    // Update is called once per frame
    void Update()
    {
        if (IsFlying)
        {
            float movement = MovementSpeed * Time.deltaTime;

            if (target != null)
            {
                transform.LookAt(target.GetProjectileTargetPosition());
            }

            Vector3 targetVector = transform.forward * movement;

            var hit = new RaycastHit();
            if (Physics.Raycast(transform.position, targetVector, out hit, movement)) //Actually hit the frame after for the effect?
            {
                var hittable = hit.transform.GetComponent<IDamageTaker>();
                if (hittable != null) //Hit something that can take damage
                {
                    if (AttemptHit(hittable)) //Returns true if it manages to damage the hit target (enemy projectiles can't hit enemies, player projectiles can't hit players)
                    {
                        Debug.Log("Hit target. The thing is " + HitsPlayers);
                        Destroy(this.gameObject);
                        return;
                    }
                }
                else
                {
                    Debug.Log("Hit wall");
                    Destroy(this.gameObject);
                    return;
                }
            }

            transform.Translate(targetVector, Space.World);

            totalMovement += movement;
            if (totalMovement > range)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private bool AttemptHit(IDamageTaker damageTaker)
    {
        if(HitsPlayers)
        {
            if (damageTaker is Player)
            {
                damageTaker.TakeHit(damage, damageType);
                return true;
            }
        }
        else
        {
            if (!(damageTaker is Player))
            {
                damageTaker.TakeHit(damage, damageType);
                return true;
            }
        }
        return false;
    }

    public void Initialize(float range, float damage, Enums.Element damageType)
    {
        this.damage = damage;
        this.range = range;
        this.damageType = damageType; //TODO: Script to set elemental partical effect to re-use projectiles when weapons have magical properties that set elemental type
        StartPosition = transform.position;
        IsFlying = true;
    }

    public void Target(IDamageTaker target) //Damage is determined on projectile creation
    {
        if (target is Player)
        {
            HitsPlayers = true;
        }

        this.target = target;
    }
}
