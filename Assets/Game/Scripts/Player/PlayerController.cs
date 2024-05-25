using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    public PlayerModel m_PlayerConfig;
    public PlayerView m_PlayerView;
    public PlayerInputService m_PlayerInputService;

    private Transform playerTansfom;
   [SerializeField] private Transform hookTransform;
    private Rigidbody playerRigidbody;


   [SerializeField] private bool isGrounded;
   [SerializeField] private bool canHook;

    private Vector3 moveDirection;
    private void Awake()
    {
        PlayerInputService.OnJumpPressed += PlayerJump;
        PlayerInputService.OnJumpReleased += OnJumpKeyRelease;
    }
    private void OnDisable()
    {
        PlayerInputService.OnJumpPressed -= PlayerJump;
        PlayerInputService.OnJumpReleased -= OnJumpKeyRelease;

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
   

        CheckingHookingState();

        CheckIsGrounded();

        HandleMovement();

    }


    void HandleInput()
    {
        moveDirection = new Vector3(m_PlayerInputService.m_Horizontal,0, m_PlayerInputService.m_Vertical);

    }
    void HandleMovement()
    {
        Vector3  currentVelocity = moveDirection.x * playerTansfom.right + moveDirection.z * playerTansfom.forward;
        currentVelocity.y = playerRigidbody.velocity.y;
        playerRigidbody.velocity = currentVelocity;
    }


    void PlayerJump()
    {
        if (isGrounded)
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

    public void CanHook(bool hookState)
    {
        canHook =  hookState;
    }

    public void SetHookableTransform(Transform hook)
    {
        hookTransform = hook;
    }

    public bool isHooked = false;
    private void CheckingHookingState()
    {
        if (canHook && hookTransform != null)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log("Space pressed");
                isHooked = true;
                playerTansfom.transform.position = hookTransform.transform.position;
                playerRigidbody.isKinematic = true;

            }

        }
     
    }

    void OnJumpKeyRelease()
    {
        if (isHooked)
        {
            playerRigidbody.isKinematic = false;
            isHooked = false;
            playerRigidbody.AddForce(Vector3.up * m_PlayerConfig.m_JumpSpeed, ForceMode.Impulse);

        }
    }
  
}
