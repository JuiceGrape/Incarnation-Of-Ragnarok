using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCaster : MonoBehaviour
{
    [SerializeField] private Enums.CastMode globalCastMode = Enums.CastMode.Normal;

    [SerializeField] private PlayerInput input = null;
    [SerializeField] private PlayerController playerController = null;
    [SerializeField] private AbilityIndicator abilityIndicator = null;

    [SerializeField] private AbilityBase[] debugAbilities = new AbilityBase[5];

    private AbilityBase cachedAbility = null;

    // Start is called before the first frame update
    void Start()
    {
        input.OnAbilityStateChanged.AddListener(OnAbilityStateChanged);
    }

    void OnClickChanged(Enums.InputState state)
    {
        input.OnMovementChanged.RemoveAllListeners();
        playerController.AttachMovementEvent();

        abilityIndicator.StopIndicator();
        Cast(cachedAbility);
    }

    void OnAbilityStateChanged(int ability, Enums.InputState state)
    {
        switch(globalCastMode)
        {
            case Enums.CastMode.Normal:
                HandleAbilityStateNormal(debugAbilities[ability], state);
                break;
            case Enums.CastMode.QuickCast:
                HandleAbilityStateQuickCast(debugAbilities[ability], state);
                break;
            case Enums.CastMode.QuickCastWithoutIndicator:
                HandleAbilityStateQuickCastNoIndicator(debugAbilities[ability], state);
                break;
            default:
                Debug.LogError("Unhandled cast type " + globalCastMode);
                break;
        }
    }

    void HandleAbilityStateNormal(AbilityBase ability, Enums.InputState state)
    {
        if (state == Enums.InputState.Down)
        {
            if (ability.CastsInstantly())
            {
                Cast(ability);
                return;
            }

            abilityIndicator.SetAbility(ability);
            abilityIndicator.ShowIndicator();
            input.OnMovementChanged.RemoveAllListeners();
            input.OnMovementChanged.AddListener(OnClickChanged);
            cachedAbility = ability;
            Debug.Log("Ability cast started");
        }
    }

    void HandleAbilityStateQuickCast(AbilityBase ability, Enums.InputState state)
    {
        if (state == Enums.InputState.Down)
        {
            if (ability.CastsInstantly())
            {
                Cast(ability);
                return;
            }

            abilityIndicator.SetAbility(ability);
            abilityIndicator.ShowIndicator();
            cachedAbility = ability;
        }
        else if (state == Enums.InputState.Up)
        {
            if (ability == cachedAbility)
            {
                abilityIndicator.StopIndicator();
                Cast(cachedAbility);
            }
        }
    }

    void HandleAbilityStateQuickCastNoIndicator(AbilityBase ability, Enums.InputState state)
    {
        if (state == Enums.InputState.Down)
        {
            Cast(ability);
        }
    }
    
    void Cast(AbilityBase ability)
    {
        playerController.Cast(ability);
    }
}
