using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;


    #region EventVariables
   // public event Action<Transform> OnCheckPointReached;
    #endregion


    public PlayerController m_PlayerController;

    public List<Transform> listOfCollectedCheckPoints;

    public bool IsInsideBonFire = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        EventsIntialize();
    }

    private void EventsIntialize()
    {
        CheckPointTrigger.OnCheckpointReached += AddCheckPoint;
    }

    private void OnDisable()
    {
        CheckPointTrigger.OnCheckpointReached -= AddCheckPoint;

    }

    [ContextMenu("SpawnPlayer")]
    public void SpawnPlayer()
    {
       Transform lastpoint = GetLastCollectedPoint();

        if (lastpoint != null) 
        {
            m_PlayerController.m_PlayerView.m_Tansform.position = lastpoint.position;
        }
    }

   private Transform GetLastCollectedPoint()
    {
        if (listOfCollectedCheckPoints.Count > 0)
        return listOfCollectedCheckPoints[listOfCollectedCheckPoints.Count - 1];

        return null;
    }

    private void AddCheckPoint(Transform transform)
    { 
      listOfCollectedCheckPoints.Add(transform);
    }



}
