using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPlatformTrigger : MonoBehaviour
{
    public static event Action OnEndPlatformTrigger;
    public bool isGoalReached = false;

    public LayerMask LayerToCheck;
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & LayerToCheck) != 0) 
        {
            if (!isGoalReached)
            {
                OnEndPlatformTrigger?.Invoke();
            }
            isGoalReached = true;
        }    
    }


}
