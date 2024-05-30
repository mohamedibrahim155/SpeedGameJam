using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerView : MonoBehaviour
{
   [Header("Dependencies")]
    public PlayerModel m_PlayerConfig;

    [Header("References")]
    public Transform m_Tansform;
    public Collider m_Collider;
    public Rigidbody m_Rigidbody;
    public Material m_Material;
    public Transform m_Skin;

    [Header("Ground Check")]
    public Transform m_GroundCheckTransform;
    public LayerMask m_WallLayer;
   
}
