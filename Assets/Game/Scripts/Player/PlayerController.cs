using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{

    public static event Action OnHookBegin;
    public static event Action OnHookEnd;

    public PlayerModel m_PlayerConfig;
    public PlayerView m_PlayerView;
    public PlayerInputService m_PlayerInputService;

   [SerializeField] private Transform playerTansfom;
   [SerializeField] private Transform hookTransform;
   [SerializeField] private Rigidbody playerRigidbody;


   [Header("Hook state")]
   [SerializeField] private bool isGrounded;
   [SerializeField] private bool canHook;
   [SerializeField] private bool isHooked = false;
   [SerializeField] private PlatformView m_Platform;

    private Vector3 moveDirection;

    public enum EHookStates
    {
        None=0,
        Hooking=1,
        Sliding=2,
    }

    public enum EPlayerState
    {
        NORMAL =1,
        FREEZE =2
    }

    public EHookStates m_HookStates;
    public EPlayerState m_PlayerState;

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

        m_HookStates = EHookStates.None;
        m_PlayerState = EPlayerState.NORMAL;
    }

    
    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {

        CheckIsGrounded();

        CheckingHookingState();
        HandleMovement();

    }


    void HandleInput()
    {
        moveDirection = new Vector3(m_PlayerInputService.m_Horizontal, 0, m_PlayerInputService.m_Vertical) * m_PlayerConfig.m_Speed;

    }
    void HandleMovement()
    {
        Vector3  currentVelocity = moveDirection.x * playerTansfom.right + moveDirection.z * playerTansfom.forward;
        currentVelocity.y = playerRigidbody.velocity.y;
        playerRigidbody.velocity = currentVelocity;
    }
    void PlayerJump()
    {
        if (canHook)
        {
            m_HookStates = EHookStates.Hooking;
        }
        else
        {
            m_HookStates = EHookStates.None;
        }
        
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

    public void SetPlatformHooked( PlatformView platform)
    {
        m_Platform = platform;
    }

    private void CheckingHookingState()
    {
        switch (m_HookStates)
        {
            case EHookStates.None:
                break;
            case EHookStates.Hooking:
                HookingState();
                break;
            case EHookStates.Sliding:
                Sliding();
                break;

        }
    }

    private void HookingState()
    {
        if (canHook && m_Platform != null)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                AttachHook(m_Platform);
            }
        }
    }

    private void AttachHook(PlatformView platform)
    {
      
        playerTansfom.transform.position = hookTransform.transform.position;
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.isKinematic = true;

        if (!isHooked)
        {
            if (platform.IsSlipperyPlatform())
            {
                platform.StartSlipperyTimer();
                platform.OnBecomeSlippery += HandlePlatformSlippery;
            }
            
        }
        isHooked = true;

    }

    private void HandlePlatformSlippery()
    {
        m_HookStates = EHookStates.Sliding;
    }

    void OnJumpKeyRelease()
    {
        if (isHooked)
        {
            m_HookStates = EHookStates.None;
            DetachtHook();
            playerRigidbody.AddForce(Vector3.up * m_PlayerConfig.m_JumpSpeed, ForceMode.Impulse);
        }
      
    }

    private void Sliding()
    {
        m_HookStates = EHookStates.None;
        DetachtHook();
    }

    public void DetachtHook()
    {
        if (m_Platform !=null)
        {
            m_Platform.OnBecomeSlippery -= HandlePlatformSlippery;
        }
        playerRigidbody.isKinematic = false;
        isHooked = false;
    }

  

}
