using System.Collections.Generic;
using UnityEngine;


public class MovePlatform : BasePlatform
{
    protected float countDownTimer = 0;
    protected int currentIndex = 0;
    protected List<Vector3> listOfPoints = new List<Vector3>();

    public override void Initialize() 
    {
        m_PlatformType = EPLatformType.MOVABLE;

        currentIndex = 0;
       
        m_PlatformHolder.transform.position = GetPointByIndex(0); // Setting the first indexpostion
    }
    public override void UpdateMovement() 
    {
        PlatformMove(GetPointByIndex(currentIndex));
    }



    public void AddPoints(Vector3 points)
    {
        listOfPoints.Add(points);
    }
    private void RemovePoints(Vector3 points) 
    {
        listOfPoints.Remove(points);
    }

    public void ClearPoints()
    {
        listOfPoints.Clear();
    }

    protected void PlatformMove(Vector3 movePoint)
    {
        if (CheckDistance(m_PlatformHolder.transform.position, movePoint))
        {
            m_PlatformHolder.transform.position = Vector3.Lerp(m_PlatformHolder.transform.position, movePoint, m_Speed * Time.deltaTime);
        }
        else
        {
            WaitTimer(m_WaitDelay);
        }
                
    }

    protected void WaitTimer(float waitTime)
    {
        if (countDownTimer >= waitTime)
        {
            countDownTimer = 0;

            OnTimerFinish();
        }
        else
        {
            countDownTimer -= -Time.deltaTime;
        }
    }

    private void OnTimerFinish()
    {
        ChangeCurrentIndexBy(1);
    }

    private bool CheckDistance(Vector3 current, Vector3 target)
    {
         return Vector3.Distance(current, target) > 0.05f;
    }

    protected Vector3 GetPointByIndex(int index)
    {
        if (index < listOfPoints.Count) { return listOfPoints[index]; }

        return Vector3.zero;
    }
    protected void ChangeCurrentIndexBy(int increment)
    {

        currentIndex += increment;

        if (currentIndex >= listOfPoints.Count) 
        {
            currentIndex = 0;
        }
    }

    public override bool IsSlippery()
    {
        return false;
    }
}
