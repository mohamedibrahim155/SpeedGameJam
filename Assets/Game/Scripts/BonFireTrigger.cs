using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonFireTrigger : MonoBehaviour
{
    public static event Action<bool> OnBonfireTriggered = delegate { };

    public Collider m_Collider;
    public LayerMask m_LayerToCheck;

    [SerializeField] private bool m_IsInsideBonFire = false;
    private void Awake()
    {
        m_Collider = GetComponent<Collider>();
    }


    public bool IsInsideBonFire()
    {
        return m_IsInsideBonFire;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & m_LayerToCheck) != 0)
        {
            m_IsInsideBonFire = true;

            OnBonfireTriggered?.Invoke(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & m_LayerToCheck) != 0)
        {
            m_IsInsideBonFire = false;

            OnBonfireTriggered?.Invoke(false);

        }
    }

}
