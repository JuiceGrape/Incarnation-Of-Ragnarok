using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float MovementSpeed = 5.0f;

    Vector3 StartPosition;

    Hittable target;

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

            if (target)
            {
                transform.LookAt(target.GetHitPosition());
            }

            Vector3 targetVector = transform.forward * movement;

            var hit = new RaycastHit();
            if (Physics.Raycast(transform.position, targetVector, out hit, movement)) //Actually hit the frame after for the effect?
            {
                Debug.Log("Hit shit");
                var hittable = hit.transform.GetComponent<Hittable>();
                if (hittable)
                {
                    hittable.TakeHit(damage, damageType);
                }
                Destroy(this.gameObject);
                return;
            }

            transform.Translate(targetVector, Space.World);

            totalMovement += movement;
            if (totalMovement > range)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void Initialize(float range, float damage, Enums.Element damageType)
    {
        this.damage = damage;
        this.range = range;
        this.damageType = damageType; //TODO: Script to set elemental partical effect to re-use projectiles when weapons have magical properties that set elemental type
        StartPosition = transform.position;
        IsFlying = true;
    }

    public void Target(Hittable target) //Damage is determined on projectile creation
    {
        this.target = target;
    }
}
