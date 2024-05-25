using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public PlayerModel playerConfigs;
    public Transform m_PlayerTansform;
    public Collider m_PlayerCollider;
    public Rigidbody m_PlayerRigidbody;

    [Header("Ground Check")]
    public Transform m_GroundCheckTransform;
 

}
