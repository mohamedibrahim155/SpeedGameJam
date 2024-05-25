using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : BasePlatform
{
    private float timer = 2;
    private float countDownTimer = 0;

    private List<Vector3> listOfPoints = new List<Vector3>();
    public override void Start() 
    {
        m_PlatformType = PlatformType.STATIC;

        m_SlipperyTimer = 2;

        Debug.Log("m_SlipperyTimer : " + m_SlipperyTimer);
    }
    public override void Move() { }
    public override void Stop() { }

    private void timerUpdate() 
    {
        if (countDownTimer>= timer)
        {
            countDownTimer = 0;
        }
        else
        {
            countDownTimer -= Time.deltaTime;
        }
                
    }

    private void InvokeSlippery() 
    {

    }
}
