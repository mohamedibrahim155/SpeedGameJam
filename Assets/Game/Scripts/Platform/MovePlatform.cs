using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

public class MovePlatform : BasePlatform
{
    private float timer = 2;
    private float countDownTimer = 0;
    private int currentIndex = 0;
    private List<Vector3> listOfPoints = new List<Vector3>();

    public int m_CurrentIndex { get { return currentIndex; } set { currentIndex = value; } }

    public float platformLerpTime = 5;


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

    private void PlatformMove(Vector3 movePoint)
    {
        if (CheckDistance(m_PlatformHolder.transform.position, movePoint))
        {
            m_PlatformHolder.transform.position = Vector3.Lerp(m_PlatformHolder.transform.position, movePoint, platformLerpTime * Time.deltaTime);
        }
        else
        {
            WaitTimer(timer);
        }
                
    }

    private void WaitTimer(float waitTime)
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

    private Vector3 GetPointByIndex(int index)
    {
        if (index < listOfPoints.Count) { return listOfPoints[index]; }

        return Vector3.zero;
    }
    private void SetIndexValue(int index)
    {
        currentIndex = index;
    }
    private Vector3 GetCurrentIndexPosition() 
    {
        return listOfPoints[currentIndex];
    }

    private void ChangeCurrentIndexBy(int increment)
    {

        currentIndex += increment;

        if (currentIndex >= listOfPoints.Count) 
        {
            currentIndex = 0;
        }
    }

    private int FindByPositon(Vector3 position)
    {
        
        for (int i =0; i< listOfPoints.Count; i++) 
        {
            if (position == listOfPoints[i])
            {
                return i;
            }
        }

        return 0;
    }


}
