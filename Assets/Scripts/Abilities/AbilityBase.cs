using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ability", menuName = "ScriptableObjects/ability/DEBUG")] //Debug: Will be replaced by proper ability scriptable objects
public class AbilityBase : ScriptableObject
{
    public Enums.AbilityTarget TargetType;

    [Tooltip("Ability casting range centered on the player")]
    public float Range;

    [Tooltip("Size of ability after being cast")]
    public float Size;

    public AnimationClip ReferenceAnimation;

    public virtual bool ShowsRangeOnPlayer()
    {
        switch(TargetType)
        {
            case Enums.AbilityTarget.Line:  //Overflow intentional
            case Enums.AbilityTarget.Cone:  //Overflow intentional
                return false;
            default:
                return true;
        }
    }

    public virtual bool CastsInstantly()
    {
        switch(TargetType)
        {
            case Enums.AbilityTarget.Self:
                return Mathf.Approximately(Range, 0.0f);
            default:
                return false;
        }
    }
}
