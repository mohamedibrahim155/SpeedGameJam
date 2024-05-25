using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public enum PlatformType
{
    STATIC = 0,
    MOVABLE = 1,

}
public  class BasePlatform
{
   public float m_SlipperyTimer { get; set; }

    protected PlatformType m_PlatformType;
    public virtual void Start() { }
    public virtual void Move() { }
    public virtual void Stop() { }
}
