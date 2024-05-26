using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StaticPlatform : BasePlatform
{

    private float countDownTimer = 0;

    private bool slippery = false;

    public override void Initialize() 
    {
        m_PlatformType = EPLatformType.STATIC;

        PlayerController.OnHookBegin += HookBegin;
        PlayerController.OnHookEnd += HookEnded;
       
    }
    public override void UpdateMovement()
    {
       
    }
    public override void Stop() 
    {
        PlayerController.OnHookBegin -= HookBegin;
        PlayerController.OnHookEnd -= HookEnded;
    }

    public override void OntriggerEnter(Collider collider) 
    {
    }
    public override void OntriggerExit(Collider collider) { }



    private void HookBegin()
    {
        slippery = true;
    }
    private void HookEnded()
    {
        slippery = false;
    }

    private void Reset()
    {
        countDownTimer = 0;

        if (slippery) { slippery = false; }
    }

    public bool isSlippery()
    {
        return slippery;
    }
    private IEnumerator Slipperytimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        slippery = false;
    }

}
