using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput input;

    [SerializeField] private float interactionDistance = 1.5f;

    private Targetable target;

    private Vector2 mousePos;

    private Enums.InputState moveButtonState = Enums.InputState.Invalid;

    private Player player;

    float timeSinceMovementClick = 0.0f;

    float timeSinceLastAttack = 0.0f;

    Animator animator;

    int GroundLayerMask;

    ProjectileLocation projectileSource;

    //Coroutine attacking;

    void Start()
    {
        if (input != null)
        {
            input.OnMousePosChanged.AddListener(OnMousePosChanged);
            input.OnMovementChanged.AddListener(OnMovementStateChanged);
        }

        GroundLayerMask = LayerMask.GetMask("Ground"); //Make sure the player can only target the ground

        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        projectileSource = GetComponentInChildren<ProjectileLocation>();
    }

    private void Update()
    {
        //Set target position if holding button. Don't attack while doing this.
        if (moveButtonState == Enums.InputState.Down)
        {
            if (timeSinceMovementClick > 0.3f)
            {
                target = null;
                MoveToMouseClick();
            }
            else
            {
                timeSinceMovementClick += Time.deltaTime;
            }
            
        }
        else if (target != null)
        {
            float targetDistance = target.IsAttackable() ? player.GetAttackRange() : interactionDistance; //TODO: Only calculate when target changes

            if (target.GetDistanceTo(transform.position) > targetDistance)
            {
                GetComponent<NavMeshAgent>().isStopped = false;
                GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
            }
            else
            {
                GetComponent<NavMeshAgent>().isStopped = true;
                transform.LookAt(target.transform); //TODO: Smooth rotation

                if (target.IsAttackable())
                {
                    if (timeSinceLastAttack >= 0.0f) //Done to make the first attack always instant
                    {
                        timeSinceLastAttack -= player.GetAttackDelay();
                        animator.SetTrigger("Attack");
                    }

                    //if (attacking == null)
                    //{
                    //    attacking = StartCoroutine(AttackTarget());
                    //}
                }
                else //Could be dead?
                {
                    ((Interactable)target).Interact();
                    target = null; //Unset target to only interact once
                    //Interact with the target
                }
            }
        }

        if (timeSinceLastAttack < 0)
            timeSinceLastAttack += Time.deltaTime; //Done so you can recover attack time without actively waiting
    }

    //IEnumerator AttackTarget()
    //{
    //    while (target != null)
    //    {
    //        animator.SetTrigger("Attack");
    //        yield return new WaitForSeconds(player.GetAttackDelay());

    //        if (target != null && !target.IsAttackable())
    //        {
    //            target = null;
    //            //TODO: Maybe break? Isn't necessary
    //        }
    //    }
    //}

    public void AttackTriggered()
    {
        var projectile = player.GetProjectile();
        if (projectile)
        {
            if (!projectileSource)
            {
                Debug.LogError("Tried to spawn a projectile but did not have a source location");
                return;
            }
            Debug.Log("Spawn Projectile");
            var spawnedProjectile = GameObject.Instantiate<Projectile>(projectile, projectileSource.transform.position, transform.rotation);
            //spawnedProjectile.Target(target as Hittable); // If no target, this returns null and is handled in the firing code
            spawnedProjectile.Initialize(player.GetAttackRange() * 1.1f, player.GetDamage(), player.GetDamageType());
        }
        else if (target != null)
        {
            var hittable = target as Hittable;
            if (hittable)
            {
                hittable.TakeHit(player.GetDamage(), player.GetDamageType());
            }
        }
    }

    void MoveToMouseClick()
    {
        GetComponent<NavMeshAgent>().isStopped = false;

        var ray = Camera.main.ScreenPointToRay(mousePos);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, GroundLayerMask))
        {
            GetComponent<NavMeshAgent>().SetDestination(hit.point);
        }
    }

    void OnMousePosChanged(Vector3 mousePos)
    {
        this.mousePos = mousePos;
    }

    void OnMovementStateChanged(Enums.InputState state)
    {
        moveButtonState = state;
        if (state == Enums.InputState.Down)
        {
            timeSinceMovementClick = 0.0f;
            if (Targetable.CurrentTarget != null)
            {
                target = Targetable.CurrentTarget; //TODO: Make function to set target for re-use
            }
            else
            {
                target = null;
                MoveToMouseClick();
            }
        }
    }
}
