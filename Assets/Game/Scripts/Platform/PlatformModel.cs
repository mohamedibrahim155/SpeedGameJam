using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName ="PlatformData", menuName = "Assets/PlatformData")]
public class PlatformModel : ScriptableObject
{
    public GameObject m_PlatformModelPrefab;

    public float m_PlatformLerpSpeed = 5.0f;

    [Header("Time")]
    public float m_StaticPlatformSlipTime = 2;
    public float m_SlipperyPlatformSlipTime = 0.5f;
    public float m_MoveablePlatformsSlipTime = 1.5f;

}
