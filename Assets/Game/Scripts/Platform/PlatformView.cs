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

    private EPLatformType currentPlatformType { get { return m_CurrentType; }  set { m_CurrentType = value; } }
    public EPLatformType m_CurrentType;

    public float m_MovablePlatformWaitDelay = 0;
    public float m_Speed = 5;
   

    public List<Vector3> m_ListOfMoveablePoints = new List<Vector3>();


    public event Action OnBecomeSlippery = delegate { };


    private bool isSlipped = false;
    private int currentTypeID = 0;
    private BasePlatform currentPlatform;
  


    [SerializeField] private  bool m_RecordPosition;
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
        AddPlatform(m_CurrentType);

        currentPlatform = GetCurrentType();

        currentPlatform.Initialize();
    }

    void AddPlatform(EPLatformType type)
    {
        switch (type)
        {
            case EPLatformType.STATIC:
                AddPlatformTypeToList(EPLatformType.STATIC, new StaticPlatform());
                break;
            case EPLatformType.MOVABLE:
                AddPlatformTypeToList(EPLatformType.MOVABLE, new MovePlatform());
                AddPointsToMoveablePlatform();
                break;
            case EPLatformType.SLIPPERY:
                AddPlatformTypeToList(EPLatformType.SLIPPERY, new SlipperyPlatform());
                break;
            case EPLatformType.MOVABLE_SLIPPERY:
                AddPlatformTypeToList(EPLatformType.MOVABLE_SLIPPERY, new MovableSlipperyPlatform());
                AddPointsToMoveablePlatform();
                break;
        }

        currentPlatformType = type;
        currentTypeID = (int)type;
    }

    private void AddPointsToMoveablePlatform()
    {
        if (m_ListOfMoveablePoints.Count <= 0) return;

        foreach (Vector3 points in m_ListOfMoveablePoints)
        {
            if (m_CurrentType == EPLatformType.MOVABLE)
            {
                ((MovePlatform)currentPlatform).AddPoints(points);
            }
            else if (m_CurrentType == EPLatformType.MOVABLE_SLIPPERY)
            {
                ((MovableSlipperyPlatform)currentPlatform).AddPoints(points);
            }
        }
    }

    void AddPlatformTypeToList(EPLatformType type, BasePlatform platform)
    {
        if (!listOfPlatforms.ContainsKey(type))
        {
            platform.SetUp(m_Config, m_Holder);
            platform.SetWaitDelay(m_MovablePlatformWaitDelay);
            platform.SetSpeed(m_Speed);

            listOfPlatforms.Add(type, platform);

            currentPlatform = platform;
        }
    }

    void RemovePlatformTypeFromList(EPLatformType type)
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
        return listOfPlatforms[currentPlatformType];
    }

    void ChangePlatformType(EPLatformType type)
    {
        if (currentPlatformType == type) return;

        if (GetPlatfromByType(type) == null)
        {
            AddPlatform(type);
        }

        currentPlatformType = type;
        currentTypeID = (int)m_CurrentType;
        currentPlatform = GetPlatfromByType(type);

    }

    void UpdatePlatformBehaviour()
    {
        switch (currentPlatformType)
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

        if (isSlipped) { ResetSlipperyState(); }
    }

    private void OnTriggerExit(Collider other)
    {

        if (isSlipped) { ResetSlipperyState(); }
    }


    public void StartSlipperyTimer()
    {
        if (m_SlipperyCoroutine != null)
        {
            StopCoroutine(m_SlipperyCoroutine);
        }

        float duration = GetSlipperyTime();

        m_SlipperyCoroutine = StartCoroutine(SlipperyTimer(duration));
    }
    public IEnumerator SlipperyTimer(float slipperyTime)
    {

        yield return new WaitForSeconds(slipperyTime);
        isSlipped = true;
        OnBecomeSlippery?.Invoke();
    }

    public void ResetSlipperyState()
    {
        if (m_SlipperyCoroutine != null)
        {
            StopCoroutine(m_SlipperyCoroutine);
        }
        isSlipped = false;
    }

    float GetSlipperyTime()
    {
        switch (m_CurrentType)
        {
            case EPLatformType.SLIPPERY:
                return m_Config.m_SlipperyPlatformSlipTime;
            case EPLatformType.MOVABLE_SLIPPERY:
                return m_Config.m_MoveablePlatformSlipTime;
        }

        return 0;
    }

    public bool IsSlipperyPlatform()
    {
        return currentPlatform.IsSlippery();
    }



    /// <summary>
    /// function for editor  recording position for platforms
    /// </summary>
    [ContextMenu("Add Current Position")]
    public void AddRecordedValueToList()
    {
        if (m_RecordPosition)
        {
            if (m_CurrentType == EPLatformType.MOVABLE || m_CurrentType == EPLatformType.MOVABLE_SLIPPERY)
            {
                m_ListOfMoveablePoints.Add(transform.position);

            }
        }
    }
}
