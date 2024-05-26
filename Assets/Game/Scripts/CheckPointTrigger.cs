using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    public static event Action<Transform> OnCheckpointReached  =  delegate { };

    public Transform SpawnPostion;

    public bool isCheckPointReached;
    public LayerMask layerToCheck;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerToCheck) != 0)
        {
            if (!isCheckPointReached)
            {
                OnCheckpointReached?.Invoke(SpawnPostion);
                isCheckPointReached = true;
            }
            
        }
    }


}
