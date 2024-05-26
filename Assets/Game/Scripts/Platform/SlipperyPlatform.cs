using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StaticPlatform : BasePlatform
{
    public override void Initialize() 
    {
        m_PlatformType = EPLatformType.STATIC;
    }
    public override void UpdateMovement()
    {
       
    }
    public override void Stop() 
    {
    }

    public override void OntriggerEnter(Collider collider) 
    {
    }
    public override void OntriggerExit(Collider collider) { }




 
}
