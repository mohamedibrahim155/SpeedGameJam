using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformView : MonoBehaviour
{
    public int m_CurrentTypeID = 0;
    public Transform m_Holder;
    public Collider m_Collider;
    public EPLatformType m_CurrentType;
    public List<Vector3> m_ListOfMoveablePoints = new List<Vector3>();

    private BasePlatform currentPlatform;
    private Dictionary<EPLatformType, BasePlatform> listOfPlatforms = new Dictionary<EPLatformType, BasePlatform>();

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



        AddPointsToMoveablePlatform();

        ChangePlatformType(EPLatformType.MOVABLE);

        currentPlatform.Initialize();
    }


    private void AddPointsToMoveablePlatform()
    {
        if (m_ListOfMoveablePoints.Count > 0)
        {
            foreach (Vector3 points in m_ListOfMoveablePoints)
            {
                ((MovePlatform)GetPlatfromByType(EPLatformType.MOVABLE)).AddPoints(points);
            }
        }
    }

    void AddPlatformType(EPLatformType type, BasePlatform platform)
    {
        if (!listOfPlatforms.ContainsKey(type)) 
        {
            platform.SetPlatformHolder(m_Holder);
            platform.SetCollider(m_Collider);

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

}
