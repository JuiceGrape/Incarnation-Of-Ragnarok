using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public void EnableDoor() //Returns true if it worked
    {
        this.gameObject.SetActive(false);
    }
}
