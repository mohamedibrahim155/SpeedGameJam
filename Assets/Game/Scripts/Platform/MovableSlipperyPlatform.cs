using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableSlipperyPlatform : MovePlatform
{
    public override void Initialize()
    {
        m_PlatformType = EPLatformType.MOVABLE_SLIPPERY;
    }
    public override void UpdateMovement()
    {
        PlatformMove(GetPointByIndex(currentIndex));
    }
    public override void Stop()
    {
    }

    public override void OntriggerEnter(Collider collider)
    {
    }
    public override void OntriggerExit(Collider collider) { }

}
