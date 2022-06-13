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



    [SerializeField] private PlayerInput input;

    [SerializeField] private float interactionDistance = 1.5f;

    int GroundLayerMask;
    Animator animator;
    ProjectileLocation projectileSource;
    Player player;
    Vector3 mousePos;

    Targetable target;

    float attackTimer = 0.0f;

    private Queue<Event> EventQueue = new Queue<Event>();

    IPlayerControllerState CurrentState;

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

        //Initialize state behaviour
        CurrentState = new PCDefaultBehaviour();
        CurrentState.Initialize(this);
        CurrentState.Entry();
    }

    private void Update()
    {
        //State behaviour
        if (CurrentState != null)
        {
            while (EventQueue.Count > 0)
            {
                HandleEvent(EventQueue.Dequeue());
            }

            CurrentState.Do(); 
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
            Debug.Log(CurrentState);
            prevState.Exit();
            CurrentState.Entry();
        }
    }

    private void GenerateEvents()
    {
        if (CurrentState is IDestinationCalculator)
        {
            if (((IDestinationCalculator)CurrentState).IsAtDestination(transform.position))
            {
                AddEvent(Event.DestinationReached);
            }
        }
    }

    public void AddEvent(Event pEvent)
    {
        EventQueue.Enqueue(pEvent);
    }

    void OnMousePosChanged(Vector3 mousePos)
    {
        this.mousePos = mousePos; //Update groundpos only on request if mousepos has changed
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
        if (Targetable.CurrentTarget != null)
        {
            target = Targetable.CurrentTarget;
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
    }

    public void EnableMovement()
    {
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
            //Do some stuff with attacking the enemy
        }
    }
}
