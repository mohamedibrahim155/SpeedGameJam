using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPlatformTrigger : MonoBehaviour
{
    public static event Action OnEndPlatformTrigger;
    public static event Action<Transform> OnFxSpawn;

    public Transform FxSpawnPosition;
    public LayerMask LayerToCheck;
    
    private bool isGoalReached;
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & LayerToCheck) != 0) 
        {
            if (!isGoalReached)
            {
                SpawnParticleFx(FxSpawnPosition);

                TriggerPlayerReached();
            }
            isGoalReached = true;
        }    
    }

    private void SpawnParticleFx(Transform fxPoint)
    {
        OnFxSpawn?.Invoke(fxPoint);
    }

    private void TriggerPlayerReached()
    {
        OnEndPlatformTrigger?.Invoke();
    }



}
