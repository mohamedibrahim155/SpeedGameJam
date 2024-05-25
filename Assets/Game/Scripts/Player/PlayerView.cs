using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public PlayerModel m_PlayerConfig;
    public Transform m_Tansform;
    public Collider m_Collider;
    public Rigidbody m_Rigidbody;

    [Header("Ground Check")]
    public Transform m_GroundCheckTransform;
 

}
