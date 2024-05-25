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
        playerTansfom = m_PlayerView.m_Tansform;
        playerRigidbody = m_PlayerView.m_Rigidbody;  
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
        Vector3  currentVelocity = playerRigidbody.velocity;

        currentVelocity.x = movementInputs.x;
        currentVelocity.z = movementInputs.y;

        playerRigidbody.velocity = currentVelocity;
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
