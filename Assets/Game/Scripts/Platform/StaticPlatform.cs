using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticPlatform : BasePlatform
{

    private float timer = 2;
    public override void Initialize() 
    {
        m_PlatformType = EPLatformType.STATIC;
    }
    public override void UpdateMovement() { }
    public override void Stop() { }
}
