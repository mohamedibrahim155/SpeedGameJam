using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "Assets/PlayerData")] 
public class PlayerModel : ScriptableObject
{
    public float m_Speed = 1f;
    public float m_JumpSpeed = 5.0f;

    [Header("Freeze State")]    
    public float m_FreezeJump = 2.5f;
    public float m_FreezSpeed = 1.0f;
    public float m_FreezeTimer = 7.5f;

    [Header("Ground Check")]
    public float m_GroundCheckDistance = 0.5f;
    public LayerMask m_WallLayer;
}
