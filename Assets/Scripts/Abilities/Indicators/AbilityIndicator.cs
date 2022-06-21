using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Yes there are multiple patterns I could use here instead of this. I felt the added complexity was not worth the easier extendability
public class AbilityIndicator : MonoBehaviour
{
    AbilityBase showingAbility = null;
    bool IndicatorIsShowing = false;
    GameObject currentIndicator = null;
    [SerializeField]
    private PlayerController playerController = null;

    [SerializeField]
    private GameObject lineIndicator = null;
    [SerializeField]
    private GameObject rangeIndicator = null;
    [SerializeField]
    private GameObject AOEIndicator = null;
    [SerializeField]
    private GameObject coneIndicator = null;

    // Update is called once per frame
    void Update()
    {
        if (IndicatorIsShowing)
        {
            rangeIndicator.transform.position = playerController.GetIndicatorPosition();

            if (showingAbility == null || currentIndicator == null)
                return;

            switch (showingAbility.TargetType)
            {
                case Enums.AbilityTarget.Self:
                    //Do nothing
                    break;
                case Enums.AbilityTarget.RangedAOE: 
                    HandleIndicatorAOE(showingAbility);
                    break;
                case Enums.AbilityTarget.Single:
                    HandleIndicatorSingle(showingAbility);
                    break;
                case Enums.AbilityTarget.Line:
                    HandleIndicatorLine(showingAbility);
                    break;
                case Enums.AbilityTarget.Cone:
                    HandleIndicatorCone(showingAbility);
                    break;
                default:
                    Debug.LogError("Unhandled ability target type: " + showingAbility.TargetType);
                    break;
            }

        }
    }

    private void HandleIndicatorSingle(AbilityBase ability)
    {
        if (ability.Size < 0.1f) //No target AOE indicator necessary on something this small
            return;

        Hittable hittable = Targetable.CurrentTarget as Hittable;

        if (hittable == null) //No target, or target is not hittable
        {
            currentIndicator.SetActive(false); //Check if it is not active? Probably redundant.
            return;
        }

        currentIndicator.SetActive(true); //Check if it is not active? Probably redundant.
        currentIndicator.transform.position = hittable.GetIndicatorOrigin();
        
    }

    private void HandleIndicatorAOE(AbilityBase ability)
    {
        if (ability.Size < 0.1f) //No target AOE indicator necessary on something this small
            return;

        var indicatorPos = playerController.GetMouseGroundPosition();

        if (indicatorPos.Equals(Vector3.negativeInfinity))
        {
            currentIndicator.SetActive(false); //Check if it is not active? Probably redundant.
            return;
        }
        else
        {
            currentIndicator.SetActive(true); //Check if it is not active? Probably redundant.
        }

        indicatorPos.y += 0.1f;

        if (Vector3.Distance(indicatorPos, playerController.transform.position) > ability.Range)
        {
            var diff = indicatorPos - playerController.transform.position;
            indicatorPos = playerController.transform.position + (diff.normalized * ability.Range);
        }


        currentIndicator.transform.position = indicatorPos;
    }

    private void HandleIndicatorLine(AbilityBase ability)
    {
        var targetPos = playerController.GetMouseGroundPosition();

        if (targetPos.Equals(Vector3.negativeInfinity))
        {
            currentIndicator.SetActive(false); //Check if it is not active? Probably redundant.
            return;
        }
        else
        {
            currentIndicator.SetActive(true); //Check if it is not active? Probably redundant.
        }

        currentIndicator.transform.position = playerController.GetIndicatorPosition();

        float targetRot = Mathf.Atan2(targetPos.z - currentIndicator.transform.position.z, targetPos.x - currentIndicator.transform.position.x) * Mathf.Rad2Deg;
        targetRot -= 90.0f;
        Vector3 newAngles = new Vector3(90.0f, 0, targetRot);
        currentIndicator.transform.rotation = Quaternion.Euler(newAngles);
    }

    private void HandleIndicatorCone(AbilityBase ability)
    {
        var targetPos = playerController.GetMouseGroundPosition();

        if (targetPos.Equals(Vector3.negativeInfinity))
        {
            currentIndicator.SetActive(false); //Check if it is not active? Probably redundant.
            return;
        }
        else
        {
            currentIndicator.SetActive(true); //Check if it is not active? Probably redundant.
        }

        var cone = currentIndicator.GetComponent<Image>();


        currentIndicator.transform.position = playerController.GetIndicatorPosition();

        float targetRot = Mathf.Atan2(targetPos.z - currentIndicator.transform.position.z, targetPos.x - currentIndicator.transform.position.x) * Mathf.Rad2Deg;
        targetRot += 90.0f + (cone.fillAmount * 180.0f);
        //Vector3 newAngles = currentIndicator.transform.rotation.eulerAngles;
        Vector3 newAngles = new Vector3(90.0f, 0, targetRot);
        currentIndicator.transform.rotation = Quaternion.Euler(newAngles);
    }

    private void SetRangeIndicator()
    {
        if (showingAbility != null)
            SetRangeIndicator(showingAbility);
    }

    private void SetRangeIndicator(AbilityBase ability)
    {
        Vector3 newScale = new Vector3(ability.Range, ability.Range, 1.0f);
        rangeIndicator.transform.localScale = newScale;
        rangeIndicator.transform.position = playerController.GetIndicatorPosition();

        rangeIndicator.SetActive(ability.ShowsRangeOnPlayer() && IndicatorIsShowing); //Set to true if it shows range on the player and the indicator is showing
    }

    public void SetAbility(AbilityBase ability)
    {
        if (ability == null)
            Debug.LogError("Ability passed to indicator is null: Not supported");

        showingAbility = ability;
        if (currentIndicator != null)
            currentIndicator.SetActive(false);

        SetRangeIndicator();

        switch (ability.TargetType)
        {
            case Enums.AbilityTarget.Self:
                //Do nothing
                break;
            case Enums.AbilityTarget.RangedAOE:
            case Enums.AbilityTarget.Single: //Overflow intentional
                currentIndicator = AOEIndicator;
                currentIndicator.transform.localScale = new Vector3(ability.Size, ability.Size, 1.0f); //TODO: Change to sprite size to accomodate nice looking scaling
                break;
            case Enums.AbilityTarget.Line:
                currentIndicator = lineIndicator;
                currentIndicator.transform.localScale = new Vector3(ability.Size, ability.Range, 1.0f); //TODO: Change to sprite size to accomodate nice looking scaling
                break;
            case Enums.AbilityTarget.Cone:
                currentIndicator = coneIndicator;
                var cone = currentIndicator.GetComponent<Image>();
                cone.fillAmount = ability.Size / 360.0f; //Size = angle
                currentIndicator.transform.localScale = new Vector3(ability.Range, ability.Range, 1.0f); //TODO: Change to sprite size to accomodate nice looking scaling
                break;
            default:
                Debug.LogError("Unhandled ability target type: " + ability.TargetType);
                break;
        }
    }

    public void ShowIndicator()
    {
        IndicatorIsShowing = true;

        //Enable relevant objects
        if (showingAbility != null && showingAbility.ShowsRangeOnPlayer())
            rangeIndicator.SetActive(true);
    }

    public void StopIndicator()
    {
        IndicatorIsShowing = false;

        //Disable relevant objects
        if (currentIndicator != null)
            currentIndicator.SetActive(false);

        rangeIndicator.SetActive(false);
    }
}
