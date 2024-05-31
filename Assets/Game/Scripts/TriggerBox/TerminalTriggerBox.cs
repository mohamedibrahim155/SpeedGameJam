using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalTriggerBox : MonoBehaviour
{
    public static event Action<bool> OnTerminalTriggered = delegate { };

    public LayerMask m_LayerToCheck;
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & m_LayerToCheck) != 0)
        {

            TerminalEntered(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & m_LayerToCheck) != 0)
        {
            TerminalEntered(false);
        }
    }

    void TerminalEntered(bool isEntered)
    {
        OnTerminalTriggered?.Invoke(isEntered);
    }

}
