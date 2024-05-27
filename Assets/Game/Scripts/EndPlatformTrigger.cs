using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPlatformTrigger : MonoBehaviour
{
    public static event Action OnEndPlatformTrigger;
    public static event Action<Transform> OnFxSpawn;
    public bool isGoalReached = false;
    public Transform FxSpawnPosition;
    public LayerMask LayerToCheck;
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & LayerToCheck) != 0) 
        {
            if (!isGoalReached)
            {
                OnFxSpawn?.Invoke(FxSpawnPosition);
                OnEndPlatformTrigger?.Invoke();
            }
            isGoalReached = true;
        }    
    }




}
