using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestinationCalculator
{
    bool IsAtDestination(Vector3 position);
}
