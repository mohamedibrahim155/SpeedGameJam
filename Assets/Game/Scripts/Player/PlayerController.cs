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

    public enum HookStates
    {
        None=0,
        Hooking=1,
        Sliding=2,

    }

    public HookStates hookStates;
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

        hookStates = HookStates.None;
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
            hookStates = HookStates.Hooking;
        }
        else
        {
            hookStates = HookStates.None;
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
        //if (canHook && hookTransform != null)
        //{
        //    if (Input.GetKey(KeyCode.Space))
        //    {
        //        if (!m_Platform.IsSlippery())
        //        {

        //            AttachHook(m_Platform);
        //        }
        //        else
        //        {
        //            DetachtHook();
        //        }

        //    }

        //}

        switch (hookStates)
        {
            case HookStates.None:
                break;
            case HookStates.Hooking:
                HookingState();
                break;
            case HookStates.Sliding:
                Sliding();
                break;

        }
     
    }

    private void HookingState()
    {
        if (canHook && hookTransform!=null)
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
            platform.StartSlipperyTimer(platform.m_Config.m_StaticPlatformSlipTime);
            platform.OnBecomeSlippery += HandlePlatformSlippery;
            
        }
        isHooked = true;

    }

    private void HandlePlatformSlippery()
    {
        Debug.Log("Sliding");
        hookStates = HookStates.Sliding;
    }

    void OnJumpKeyRelease()
    {
        if (isHooked)
        {
            DetachtHook();
            hookStates = HookStates.None;
            playerRigidbody.AddForce(Vector3.up * m_PlayerConfig.m_JumpSpeed, ForceMode.Impulse);
        }
      
    }

    private void Sliding()
    {

        hookStates = HookStates.None;

        playerRigidbody.isKinematic = false;
        isHooked = false;
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
