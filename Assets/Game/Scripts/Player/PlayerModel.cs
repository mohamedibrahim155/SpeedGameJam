using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "Assets/PlayerData")] 
public class PlayerModel : ScriptableObject
{
    public float m_Speed = 1f;
    public float m_JumpSpeed = 5.0f;

    [Header("Ground Check")]
    public float m_GroundCheckDistance = 0.5f;
}
