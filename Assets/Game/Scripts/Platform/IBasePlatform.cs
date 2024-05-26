using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class BasePlatform
{
    public float m_WaitDelay = 0;
    public float m_Speed = 1;
    public Transform m_PlatformHolder;
    public PlatformModel m_PlatformConfig;
    public EPLatformType m_PlatformType;

    public void SetUp(PlatformModel config, Transform holder)
    {
        m_PlatformConfig = config;
        m_PlatformHolder = holder;
    }

    public void SetWaitDelay(float timer)
    {
        m_WaitDelay = timer;
    }
    public void SetSpeed(float speed)
    {
        m_Speed = speed;
    }

    public virtual void Initialize() { }
    public virtual void UpdateMovement() { }
    public virtual void Stop() { }

    public virtual void OntriggerEnter(Collider collider) { }
    public virtual void OntriggerExit(Collider collider) { }
}
