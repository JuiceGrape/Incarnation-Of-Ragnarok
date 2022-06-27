using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public enum Event
    {
        TargetClicked,
        TargetDied,
        GroundClicked,
        ClickHeld,
        DestinationReached,
        TargetLeftReach,
        AbilityActivated,
        AbilityEnded
    }

    public enum State
    {
        DefaultBehaviour,
        AbilityUsing,
        Idle,
        Attacking,
        MovingToTarget,
        MovingToLocation
    }


    [SerializeField] private PlayerInput input = null;
    [SerializeField] private AbilityIndicator indicator = null;

    [SerializeField] private float interactionDistance = 1.5f;

    int GroundLayerMask;
    Animator animator;
    
    IndicatorOrigin indicatorOrigin;
    Player player;
    Vector3 mousePos;
    BasicAttackHandler basicAttack;

    Targetable target;

    public bool IsCasting = false;

    public float attackTimer = 0.0f;

    private Queue<Event> EventQueue = new Queue<Event>();

    public IPlayerControllerState CurrentState;
    

    void Start()
    {
        input.OnMousePosChanged.AddListener(OnMousePosChanged);
        AttachMovementEvent();

        GroundLayerMask = LayerMask.GetMask("Ground"); //Make sure the player can only target the ground

        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        basicAttack = GetComponent<BasicAttackHandler>();


        //Initialize state behaviour
        CurrentState = new PCIdle();
        CurrentState.Initialize(this);
        CurrentState.Entry();

        indicatorOrigin = GetComponentInChildren<IndicatorOrigin>();
        if (!indicatorOrigin)
            Debug.LogError("Player has no indicator origin. Defaulting to own location as origin"); //TODO: Maybe warning instead?
    }

    private void Update()
    {
        //State behaviour
        if (!IsCasting)
        {
            CurrentState.Do();

            while (EventQueue.Count > 0)
            {
                HandleEvent(EventQueue.Dequeue());
            }
        }

        if (attackTimer < 0)
            attackTimer += Time.deltaTime;

        //Generate events based on logic
        GenerateEvents();
    }

    private void HandleEvent(Event pEvent)
    {
        var prevState = CurrentState;

        CurrentState = CurrentState.HandleEvent(pEvent);

        if (prevState != CurrentState)
        {
            prevState.Exit();
            CurrentState.Entry();
        }
    }

    private void GenerateEvents()
    {
        //Empty
    }

    public void AddEvent(Event pEvent)
    {
        if (!IsCasting)
            EventQueue.Enqueue(pEvent);
    }

    void OnMousePosChanged(Vector3 mousePos)
    {
        this.mousePos = mousePos; //Calculate on request: Most likely (way) more mouse updates than requests for ground position
    }

    void OnMovementStateChanged(Enums.InputState state)
    {
        switch (state)
        {
            case Enums.InputState.Down:
                OnClick();
                break;
            case Enums.InputState.Up:
                //Do nothing
                break;
            case Enums.InputState.Held:
                AddEvent(Event.ClickHeld);
                break;
        }
    }

    void OnClick()
    {
        OnCastCanceled();

        if (Targetable.CurrentTarget != null)
        {
            target = Targetable.CurrentTarget;
            basicAttack.SetTarget(target as IDamageTaker);
            AddEvent(Event.TargetClicked);
        }
        else
        {
            //TODO: Check if clicking ground
            AddEvent(Event.GroundClicked);
        }
    }


    //Container functions
    public void DisableMovement()
    {
        GetComponent<NavMeshAgent>().isStopped = true;
        animator.SetBool("IsMoving", false);
    }

    public void EnableMovement()
    {
        animator.SetBool("IsMoving", true);
        GetComponent<NavMeshAgent>().isStopped = false;
    }

    public Targetable GetTarget()
    {
        return target;
    }

    public Vector3 GetMouseGroundPosition()
    {
        var ray = Camera.main.ScreenPointToRay(mousePos);

        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, GroundLayerMask))
        {
            return hit.point;
        }
        else
        {
            return Vector3.negativeInfinity; //This means error
        }
    }

    public void SetDestination(Vector3 position)
    {
        GetComponent<NavMeshAgent>().SetDestination(position);
    }

    public float GetInteractionDistance()
    {
        return interactionDistance;
    }

    public void Interact()
    {
        var interactable = target as Interactable;
        if (interactable != null)
        {
            interactable.Interact();
        }
    }

    public void AttackTarget()
    {
        var hittable = target as Hittable;
        if (hittable != null)
        {
            if (attackTimer >= 0.0f)
            {
                animator.SetTrigger("Attack");
                attackTimer -= player.GetAttackDelay();
            }
        }
    }

    public Player GetAttachedPlayer()
    {
        return player;
    }

    //External things
    public void Cast(string AbilityStateName)
    {
        OnCastStart();
        //animator.SetTrigger("UseAbility");
        animator.Play(AbilityStateName);
    }

    public void OnCastStart()
    {
        IsCasting = true;
        DisableMovement();
    }

    public void OnCastEnd() //Should be called at the end of casting by the casting animation or other sources
    {
        IsCasting = false;
        CurrentState.Entry();
    }

    public void OnCastCanceled()
    {
        IsCasting = false; 
    }

    public Vector3 GetIndicatorPosition()
    {
        return indicatorOrigin.transform.position;
    }

    public void AttachMovementEvent()
    {
        input.OnMovementChanged.AddListener(OnMovementStateChanged);
    }
}
