using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlipperyPlatform : BasePlatform
{
    public override void Initialize() 
    {
        m_PlatformType = EPLatformType.SLIPPERY;
    }
    public override void UpdateMovement()
    {
       
    }


    public override bool IsSlippery()
    {
        return true;
    }


}
