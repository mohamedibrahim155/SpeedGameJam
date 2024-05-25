using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;


public  class BasePlatform
{
   public float m_SlipperyTimer { get; set; }

    public Transform m_PlatformHolder;
    public Collider m_Collider;


    public EPLatformType m_PlatformType;

    public void SetPlatformHolder(Transform holder)
    {
        m_PlatformHolder = holder;
    }
    public void SetCollider(Collider collider)
    {
        m_Collider = collider;
    }

    public virtual void Initialize() { }
    public virtual void UpdateMovement() { }
    public virtual void Stop() { }
}
