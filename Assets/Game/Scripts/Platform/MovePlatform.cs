using System.Collections;
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

        m_SlipperyTimer = 2;
        currentIndex = 0;
       
        m_PlatformHolder.transform.position = GetPointByIndex(0); // Setting the first indexpostion
    }
    public override void UpdateMovement() 
    {
        PlatformMove(GetPointByIndex(currentIndex));
    }
    public override void Stop() { }


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
            m_PlatformHolder.transform.position = Vector3.Lerp(m_PlatformHolder.transform.position, movePoint, m_PlatformConfig.m_PlatformLerpSpeed * Time.deltaTime);
        }
        else
        {
            WaitTimer(m_PlatformConfig.m_MovePlatformWaitTimeDelay);
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

    protected void OnTimerFinish()
    {
        ChangeCurrentIndexBy(1);
    }

    protected bool CheckDistance(Vector3 current, Vector3 target)
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

    public override void OntriggerEnter(Collider collider)
    {
    }

    public override void OntriggerExit(Collider collider) { }
}
