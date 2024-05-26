using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformView : MonoBehaviour
{
    public PlatformModel m_Config;
    public Transform m_Holder;
    public Collider m_Collider;
    public EPLatformType m_CurrentType;
    public List<Vector3> m_ListOfMoveablePoints = new List<Vector3>();


    public event Action OnBecomeSlippery = delegate { };


    private bool isSlippery = false;
    private int m_CurrentTypeID = 0;
    private float slipperyTimer = 0;

    private BasePlatform currentPlatform;
    private Dictionary<EPLatformType, BasePlatform> listOfPlatforms = new Dictionary<EPLatformType, BasePlatform>();
    private Coroutine m_SlipperyCoroutine = null;



    void Start()
    {
        Initilize();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlatformBehaviour();
    }

    private void Initilize()
    {
        AddPlatformType(EPLatformType.STATIC, new StaticPlatform());
        AddPlatformType(EPLatformType.MOVABLE, new MovePlatform());
        AddPlatformType(EPLatformType.SLIPPERY, new SlipperyPlatform());
        AddPlatformType(EPLatformType.MOVABLE_SLIPPERY, new MovableSlipperyPlatform());



        AddPointsToMoveablePlatform();

        currentPlatform = GetCurrentType();

        currentPlatform.Initialize();
    }


    private void AddPointsToMoveablePlatform()
    {
        if (m_ListOfMoveablePoints.Count > 0)
        {
            if (m_CurrentType == EPLatformType.MOVABLE)
            {
                foreach (Vector3 points in m_ListOfMoveablePoints)
                {
                    ((MovePlatform)GetPlatfromByType(EPLatformType.MOVABLE)).AddPoints(points);
                }
            }
            else if (m_CurrentType == EPLatformType.MOVABLE_SLIPPERY)
            {
                foreach (Vector3 points in m_ListOfMoveablePoints)
                {
                    ((MovableSlipperyPlatform)GetPlatfromByType(EPLatformType.MOVABLE_SLIPPERY)).AddPoints(points);
                }
            }
           
        }
    }

    void AddPlatformType(EPLatformType type, BasePlatform platform)
    {
        if (!listOfPlatforms.ContainsKey(type))
        {
            platform.SetPlatformHolder(m_Holder);
            platform.SetCollider(m_Collider);
            platform.SetPlatformConfig(m_Config);

            listOfPlatforms.Add(type, platform);

            currentPlatform = platform;
        }
    }

    void RemovePlatformType(EPLatformType type)
    {
        if (listOfPlatforms.ContainsKey(type))
        {
            listOfPlatforms.Remove(type);
        }
    }

    BasePlatform GetPlatfromByType(EPLatformType type)
    {
        return listOfPlatforms[type];
    }

    BasePlatform GetCurrentType()
    {
        return listOfPlatforms[m_CurrentType];
    }

    void ChangePlatformType(EPLatformType type)
    {
        m_CurrentType = type;
        m_CurrentTypeID = (int)m_CurrentType;
        currentPlatform = GetPlatfromByType(type);

    }

    void UpdatePlatformBehaviour()
    {
        switch (m_CurrentType)
        {
            case EPLatformType.STATIC:
                ChangePlatformType(EPLatformType.STATIC);
                break;
            case EPLatformType.MOVABLE:
                ChangePlatformType(EPLatformType.MOVABLE);

                break;
            case EPLatformType.SLIPPERY:
                ChangePlatformType(EPLatformType.SLIPPERY);

                break;
            case EPLatformType.MOVABLE_SLIPPERY:
                ChangePlatformType(EPLatformType.MOVABLE_SLIPPERY);

                break;
        }

        if (currentPlatform != null)
        {
            currentPlatform.UpdateMovement();

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        currentPlatform.OntriggerEnter(other);

        if (isSlippery) { ResetSlippery(); }
    }

    private void OnTriggerExit(Collider other)
    {
        currentPlatform.OntriggerExit(other);

        if (isSlippery) { ResetSlippery(); }
    }


    public void StartSlipperyTimer()
    {
        if (m_SlipperyCoroutine != null)
        {
            StopCoroutine(m_SlipperyCoroutine);
        }

        float dureation = GetSlipperyTime();

        m_SlipperyCoroutine = StartCoroutine(SlipperyTimer(dureation));
    }
    public IEnumerator SlipperyTimer(float slipperyTime)
    {

        yield return new WaitForSeconds(slipperyTime);
        isSlippery = true;
        OnBecomeSlippery?.Invoke();
    }

    public bool isSlip()
    {
        return isSlippery;
    }

    public void ResetSlippery()
    {
        if (m_SlipperyCoroutine != null)
        {
            StopCoroutine(m_SlipperyCoroutine);
        }
        isSlippery = false;
    }

    float GetSlipperyTime()
    {
        switch (m_CurrentType)
        {
            case EPLatformType.SLIPPERY:
                return m_Config.m_SlipperyPlatformSlipTime;
                break;
            case EPLatformType.MOVABLE_SLIPPERY:
                return m_Config.m_MoveablePlatformSlipTime;
                break;
        }

        return 0;
    }

    public bool IsSlipperyPlatform()
    {
        switch (m_CurrentType)
        {
            case EPLatformType.STATIC:
                return false;
                break;
            case EPLatformType.MOVABLE:
                return false;
                break;

            case EPLatformType.SLIPPERY:
                return true;
                break;
            case EPLatformType.MOVABLE_SLIPPERY:
                return true;
                break;
        }

        return false;
    }
}
