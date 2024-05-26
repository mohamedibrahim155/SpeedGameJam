using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class BasePlatform
{
    public float m_SlipperyTimer = 0;
    public Transform m_PlatformHolder;
    public Collider m_Collider;
    public PlatformModel m_PlatformConfig;
    public EPLatformType m_PlatformType;

    public void SetPlatformHolder(Transform holder)
    {
        m_PlatformHolder = holder;
    }
    public void SetCollider(Collider collider)
    {
        m_Collider = collider;
    }

    public void SetPlatformConfig(PlatformModel config)
    {
        m_PlatformConfig = config;
    }

    public virtual void Initialize() { }
    public virtual void UpdateMovement() { }
    public virtual void Stop() { }

    public virtual void OntriggerEnter(Collider collider) { }
    public virtual void OntriggerExit(Collider collider) { }
}
