using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerModel m_PlayerConfig;
    public PlayerView m_PlayerView;
    public PlayerInputService m_PlayerInputService;

    private Transform playerTansfom;
    private Rigidbody playerRigidbody;
   [SerializeField] private bool isGrounded;

    private Vector2 movementInputs;
    private void Awake()
    {
        PlayerInputService.OnJumpPressed += PlayerJump;
    }
    private void OnDisable()
    {
        PlayerInputService.OnJumpPressed -= PlayerJump;
    }
    void Start()
    {
        playerTansfom = m_PlayerView.m_PlayerTansform;
        playerRigidbody = m_PlayerView.m_PlayerRigidbody;  
    }

    
    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }


    void HandleInput()
    {
        movementInputs = new Vector2(m_PlayerInputService.m_Horizontal, m_PlayerInputService.m_Vertical).normalized * m_PlayerConfig.m_Speed *Time.deltaTime;

    }
    void HandleMovement()
    {
        Vector3  currentVecocity = playerRigidbody.velocity;

        currentVecocity.x = movementInputs.x;
        currentVecocity.z = movementInputs.y;

        playerRigidbody.velocity = currentVecocity;
    }


    void PlayerJump()
    {
        if (CheckIsGrounded())
        {
            playerRigidbody.AddForce(Vector3.up * m_PlayerConfig.m_JumpSpeed, ForceMode.Impulse);
        } 
    }

    bool CheckIsGrounded()
    {
         isGrounded = Physics.Raycast(m_PlayerView.m_GroundCheckTransform.position,Vector3.down, m_PlayerConfig.m_GroundCheckDistance);
       
        return isGrounded;
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
    
        Gizmos.DrawLine(m_PlayerView.m_GroundCheckTransform.position, m_PlayerView.m_GroundCheckTransform.position + Vector3.down * m_PlayerConfig.m_GroundCheckDistance);
    }

}
