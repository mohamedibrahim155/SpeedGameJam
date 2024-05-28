using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public enum EPlayerState
{
    NORMAL = 1,
    ENTER_FREEZEING = 2,
    FREEZE = 3
}
public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    public PlayerModel m_PlayerConfig;
    public PlayerView m_PlayerView;
    public PlayerInputService m_PlayerInputService;



   [Header("Player References")]
    private Transform playerTansfom;
    private Transform hookTransform;
    private Transform m_Parent;
    private Rigidbody playerRigidbody;

 

   [SerializeField] private bool isGrounded;

    public enum EHookStates
    {
        None = 0,
        Hooking = 1,
        Sliding = 2,
    }
    [Header("Hook state")]
    [SerializeField] private bool canHook;
   [SerializeField] private bool isHooked;
   [SerializeField] private PlatformView m_Platform;
   [SerializeField] private EHookStates m_CurrentHookState;



    [Header("Freeze States")]
    [SerializeField] private bool isInsideBonFire;
    public EPlayerState m_CurrentPlayerState;
    private Coroutine FreezeCoroutine;

    [Header("Inputs")]
    [SerializeField] private bool m_IsJumpKeyPressed = false;
    [SerializeField] private bool restrictVerticalAxis;
    private Vector3 moveDirection;



    private void Awake()
    {
        PlayerInputService.OnJumpPressed += OnJumpkeyPressed;
        PlayerInputService.OnJumpReleased += OnJumpKeyRelease;

        BonFireTrigger.OnBonfireTriggered += OnBonFireCollision;
    }
    private void OnDisable()
    {
        PlayerInputService.OnJumpPressed -= OnJumpkeyPressed;
        PlayerInputService.OnJumpReleased -= OnJumpKeyRelease;
        BonFireTrigger.OnBonfireTriggered -= OnBonFireCollision;

    }
    void Start()
    {

        SetUpConfigs(m_PlayerView.m_Tansform, m_PlayerView.m_Rigidbody);

        m_CurrentHookState = EHookStates.None;

        SetPlayerState(EPlayerState.ENTER_FREEZEING);

       // CheckPlayerState(m_CurrentPlayerState);
    }

    void SetUpConfigs(Transform playerTransform, Rigidbody rigidbody)
    {
        playerTansfom = playerTransform;
        m_Parent = playerTransform.parent;
        playerRigidbody = rigidbody;
    }
    
    // Update is called once per frame
    void Update()
    {
        HandleInput();

        PlayerSkinRotation();
    }

    private void FixedUpdate()
    {
      
        CheckIsGrounded();

        CheckingHookingState();
        HandleMovement();

     //   RotatePlayerTowardsWalls();

    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawLine(m_PlayerView.m_GroundCheckTransform.position, m_PlayerView.m_GroundCheckTransform.position + Vector3.down * m_PlayerConfig.m_GroundCheckDistance);

        Gizmos.color = Color.green;

        //Gizmos.DrawWireSphere(playerTansfom.position + playerTansfom.forward*100 , 5);


    }

    #region Movement
    void HandleInput()
    {

        moveDirection = new Vector3(m_PlayerInputService.m_Horizontal, 0, m_PlayerInputService.m_Vertical) * GetMoveSpeed();

        if (restrictVerticalAxis)
        {
            moveDirection.z = 0;
        }
    }
    void HandleMovement()
    {
        Vector3  currentVelocity = moveDirection.x * playerTansfom.right + moveDirection.z * playerTansfom.forward;
        currentVelocity.y = playerRigidbody.velocity.y;
        playerRigidbody.velocity = currentVelocity;
    }
    void OnJumpkeyPressed()
    {
        m_IsJumpKeyPressed = true;
        if (canHook)
        {
            m_CurrentHookState = EHookStates.Hooking;
        }
        //else
        // {
        // m_HookStates = EHookStates.None;
        //  }

        if (CheckIsGrounded())
        {
            playerRigidbody.AddForce(Vector3.up * GetJumpSpeed(), ForceMode.Impulse);
        }
    }

    void OnJumpKeyRelease()
    {
        m_IsJumpKeyPressed = false;
        if (isHooked)
        {
           m_CurrentHookState = EHookStates.None;

            DetachHook();
            playerRigidbody.AddForce(Vector3.up * GetJumpSpeed(), ForceMode.Impulse);
        }

    }

    bool CheckIsGrounded()
    {
         isGrounded = Physics.Raycast(m_PlayerView.m_GroundCheckTransform.position,Vector3.down, m_PlayerConfig.m_GroundCheckDistance, m_PlayerConfig.m_WallLayer);
       
        return isGrounded;
    }

    bool IsInAir()
    {
        return (!isGrounded);
    }

    #endregion


    #region Hook's Collision
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
    #endregion

    #region HookingState
    void SetHookState(EHookStates state)
    {
        m_CurrentHookState = state;
    }
    private void CheckingHookingState()
    {
        switch (m_CurrentHookState)
        {
            case EHookStates.None:
                break;
            case EHookStates.Hooking:
                HookingState();
                break;
            case EHookStates.Sliding:
                //Sliding();
                Sliding2();
                break;

        }
    }

    private void HookingState()
    {
        if (canHook && m_Platform != null) 
        {
            if (JumpKeyHold())
            {
               // AttachHook2(m_Platform);
                AttachHook(m_Platform);
            }
        }
    }

    private bool JumpKeyHold()
    {
        return m_IsJumpKeyPressed;
    }
 
    private void AttachHook(PlatformView platform)
    {

        playerTansfom.transform.position = hookTransform.transform.position;
       // playerTansfom.transform.position = Vector3.Lerp(playerTansfom.transform.position, hookTransform.transform.position, 10 * Time.deltaTime);


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

    private void AttachHook2(PlatformView platform)
    {
        if (isHooked) 
        {
            return;
        }

        playerTansfom.parent = hookTransform;
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.isKinematic = true;


        if (platform.IsSlipperyPlatform())
        {
            platform.StartSlipperyTimer();
            platform.OnBecomeSlippery += HandlePlatformSlippery;
        }

        
        isHooked = true;

    }


    #endregion


    #region Sliding State
    private void HandlePlatformSlippery()
    {
        m_CurrentHookState = EHookStates.Sliding;
    }
    private void Sliding()
    {   
    
        DetachHook();

        if ( JumpKeyHold())
        {
            m_CurrentHookState = EHookStates.Hooking;
        }
        else
        {
            m_CurrentHookState = EHookStates.None;
        }
    }

    private void Sliding2()
    {
        m_CurrentHookState = EHookStates.None;
        DetachHook();
    }
    public void DetachHook()
    {
        if (m_Platform !=null)
        {
            m_Platform.OnBecomeSlippery -= HandlePlatformSlippery;
        }
        playerTansfom.parent = m_Parent;
        playerRigidbody.isKinematic = false;
        isHooked = false;
        canHook = false;


    }
    #endregion

    void WallHit()
    {
        bool isWall = false;

        RaycastHit hit;
        isWall = Physics.Raycast(playerTansfom.transform.position + new Vector3(0,-0.25f,0), 
            playerTansfom.forward, 
            out hit, Mathf.Infinity, m_PlayerConfig.m_WallLayer);

        if (isWall) 
        {
            if (hit.collider!=null)
            {
                Vector3 direction = hit.point - playerTansfom.position;
                direction.Normalize();
                 direction.y = 0;
                Quaternion playerRotation = Quaternion.LookRotation(direction);


                playerTansfom.rotation = Quaternion.Slerp(playerTansfom.rotation,playerRotation, 2*Time.deltaTime);

                Debug.DrawLine(playerTansfom.transform.position + new Vector3(0, -0.25f, 0), hit.point, Color.green);
                Debug.Log("WallHit true");
            }
           
        }


    }

    void RotatePlayerTowardsWalls()
    {
        float castRadius = 0.5f; // Adjust radius based on player size and wall proximity needs
        Vector3 castOrigin = playerTansfom.position; // Adjust offset depending on player model
        Vector3 castDirection = playerTansfom.forward;
        float castDistance = Mathf.Infinity; // Adjust distance based on detection range

        RaycastHit hit;
        if (Physics.SphereCast(castOrigin, castRadius, castDirection, out hit, castDistance, m_PlayerConfig.m_WallLayer))
        {
            Debug.DrawLine(playerTansfom.position, hit.point);

            // Ensure only walls with appropriate normals are considered
            if (Mathf.Abs(Vector3.Dot(hit.normal, Vector3.up)) > 0.5f) // Adjust threshold for acceptable wall angles
            {
                return; // Skip rotation if not a suitable wall
            }

            // Calculate the target rotation based on wall normal (ignoring Y-axis)
            Vector3 targetDirection = Vector3.ProjectOnPlane(hit.normal, Vector3.up).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // Smoothly rotate player towards the target rotation
            playerTansfom.rotation = Quaternion.Slerp(playerTansfom.rotation, targetRotation, 2 * Time.deltaTime);

        }
    }

    void PlayerSkinRotation()
    {
        #region Old


        //if ((m_IsJumpKeyPressed))
        //{

        //    float targetRotation = isHooked ? 180 : angle * -1.0f;
        //    m_PlayerView.m_Skin.rotation = Quaternion.Euler(0, targetRotation, 0);
        //}
        //else
        //{
        //    float targetRotation3 = isGrounded ? angle * -1.0f : (IsInAir() && !isHooked) ? (horizontal == 0) ? 180 : angle * -1.0f : angle * -1.0f;
        //    m_PlayerView.m_Skin.rotation = Quaternion.Euler(0, targetRotation3, 0);

        //}
        #endregion


        float horizontal = m_PlayerInputService.m_Horizontal;
        float angle = horizontal * 90.0f;
        float targetRotationY = 0.0f;  

        if (m_IsJumpKeyPressed)
        {
            targetRotationY = isHooked ? 180.0f : angle * -1.0f;
        }
        else
        {
            targetRotationY = (isGrounded || (IsInAir() && !isHooked && horizontal != 0)) ? angle * -1.0f : 180.0f;
        }

        m_PlayerView.m_Skin.rotation = Quaternion.Euler(0, targetRotationY, 0);
    





}

    #region BonFireState
    void OnBonFireCollision(bool isBonFire)
    {
        isInsideBonFire =  isBonFire;

        m_CurrentPlayerState = isInsideBonFire ? EPlayerState.NORMAL : EPlayerState.ENTER_FREEZEING;

        SetPlayerState(isInsideBonFire ? EPlayerState.NORMAL : EPlayerState.ENTER_FREEZEING);

       
    }


    void CheckPlayerState(EPlayerState state)
    {
        switch (state) 
        {
            case EPlayerState.NORMAL:
                StopFreezeTimer();
                break;

            case EPlayerState.ENTER_FREEZEING:
                StartFreezeTimer(m_PlayerConfig.m_FreezeTimer);
                break;

            case EPlayerState.FREEZE:
                FreezeState();
                break;   
        }
    }

    private void StartFreezeTimer(float duration)
    {
        if (FreezeCoroutine!=null)
        {
            StopCoroutine(FreezeCoroutine);
        }
        FreezeCoroutine = StartCoroutine(FreezeTimer(duration));
    }

    private void StopFreezeTimer() 
    {
        if (FreezeCoroutine != null)
        {
            StopCoroutine(FreezeCoroutine);
        }
        FreezeCoroutine =  null;
    }

    private IEnumerator FreezeTimer(float duration)
    {
        yield return new WaitForSeconds(duration);

       SetPlayerState(EPlayerState.FREEZE);

       // CheckPlayerState(m_CurrentPlayerState);
    }

    private void SetPlayerState(EPlayerState state)
    {
        m_CurrentPlayerState = state;

        CheckPlayerState(m_CurrentPlayerState);
    }

    private void FreezeState() 
    {
        SetMaterialColor(new Color(1.0f, 0, 0, 0.5f));
    }

  

    bool IsInsideBonFire()
    {
        return isInsideBonFire;
    }
    #endregion

    private float GetJumpSpeed()
    {
        if (m_CurrentPlayerState == EPlayerState.FREEZE)
            return m_PlayerConfig.m_FreezeJump;

        return m_PlayerConfig.m_JumpSpeed;
    }

    private float GetMoveSpeed()
    {
        if (m_CurrentPlayerState == EPlayerState.FREEZE)
            return m_PlayerConfig.m_FreezSpeed;

        return m_PlayerConfig.m_Speed;
    }

    private void SetMaterialColor(Color color)
    {
        m_PlayerView.m_Material.color = color;
    }
}
