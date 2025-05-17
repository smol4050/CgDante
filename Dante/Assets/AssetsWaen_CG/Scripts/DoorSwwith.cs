using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwwith : MonoBehaviour, IInteractuable
{

    public NewBehaviourScript myDoor;


    public void ActivarObjeto()
    {
        myDoor.ActivarObjeto();
    }
}
