
using UnityEngine;

public class PlayerColliderCheck : MonoBehaviour
{
    public LayerMask layerToCheck;
    public Transform m_HookTransform;

    [Header("Dependencies")]
    public PlayerController m_Controller;
    public PlatformView m_Platform;

    private void OnTriggerEnter(Collider collider)
    {
        if (((1 << collider.gameObject.layer) & layerToCheck) != 0)
        {
            m_Controller.CanHook(true);
            m_HookTransform = collider.transform.GetChild(0);
            m_Controller.SetHookableTransform(m_HookTransform);

            if (collider.TryGetComponent<PlatformView>(out PlatformView platform))
            {
                m_Controller.SetPlatformHooked(platform);
            }

        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (((1 << collider.gameObject.layer) & layerToCheck) != 0)
        {
            if (m_HookTransform)
            {
                m_Controller.CanHook(false);
                m_HookTransform = null;
                m_Controller.SetHookableTransform(null);
                m_Controller.SetPlatformHooked(null);

            }
        }
    }
}
