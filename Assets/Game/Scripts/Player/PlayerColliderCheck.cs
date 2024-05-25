using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderCheck : MonoBehaviour
{
    public LayerMask layerToCheck;

    public Transform m_HookTransform;
    public PlayerController controller;


    private void Start()
    {
       // controller = GetComponentInParent<PlayerController>();
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Platform"))
        {
           Debug.Log(collider.gameObject.name);

            controller.CanHook(true);
            m_HookTransform = collider.transform.GetChild(0);

            controller.SetHookableTransform(m_HookTransform);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Platform"))
        {
            if (m_HookTransform)
            {
                controller.CanHook(false);

                m_HookTransform = null;
                controller.SetHookableTransform(m_HookTransform);
              

            }
        }
    }
}
