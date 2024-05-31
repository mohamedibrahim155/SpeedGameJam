using UnityEngine;

public  class BasePlatform
{
    protected float m_WaitDelay = 0;
    protected float m_Speed = 1;
    protected Transform m_PlatformHolder;
    protected PlatformModel m_PlatformConfig;
    protected EPLatformType m_PlatformType;

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

    public virtual bool IsSlippery()
    { 
        return false;
    }
}
