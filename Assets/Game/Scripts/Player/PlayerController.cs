using System;
using System.Collections;
using UnityEngine;

public enum EPlayerState
{
    NORMAL = 1,
    ENTER_SLOWSTATE = 2,
    SLOW_STATE = 3
}
public enum EHookStates
{
    NONE = 0,
    HOOKING = 1,
    SLIDING = 2,
}

public class PlayerController : MonoBehaviour
{
    public static event Action OnPlayerJumped = delegate { };
    public static event Action<Transform> OnPlayerHooked = delegate { };

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
        BonFireTrigger.OnTerminalTriggered += TerminalCollided;
    }
    private void OnDisable()
    {
        PlayerInputService.OnJumpPressed -= OnJumpkeyPressed;
        PlayerInputService.OnJumpReleased -= OnJumpKeyRelease;
        BonFireTrigger.OnTerminalTriggered -= TerminalCollided;

    }
    void Start()
    {
        Initilize();
    }

    private void Initilize()
    {
        SetUpConfigs(m_PlayerView.m_Tansform, m_PlayerView.m_Rigidbody);

        m_CurrentHookState = EHookStates.NONE;

        SetPlayerState(EPlayerState.ENTER_SLOWSTATE);
}

    void SetUpConfigs(Transform playerTransform, Rigidbody rigidbody)
    {
        playerTansfom = playerTransform;
        m_Parent = playerTransform.parent;
        playerRigidbody = rigidbody;
    }
   
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
            m_CurrentHookState = EHookStates.HOOKING;
        }
        //else
        // {
        // m_HookStates = EHookStates.None;
        //  }

        if (CheckIsGrounded())
        {
            Jump();
        }
    }

    void OnJumpKeyRelease()
    {
        m_IsJumpKeyPressed = false;
        if (isHooked)
        {
           m_CurrentHookState = EHookStates.NONE;

            DetachHook();

            Jump();
        }

    }

    void Jump()
    {
        OnPlayerJumped?.Invoke();

        playerRigidbody.AddForce(Vector3.up * GetJumpSpeed(), ForceMode.Impulse);
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
            case EHookStates.NONE:
                break;
            case EHookStates.HOOKING:
                HookingState();
                break;
            case EHookStates.SLIDING:
                //Sliding();
                Sliding2();
                break;

        }
    }

    private void HookingState()
    {
        if (canHook && m_Platform != null) 
        {
            if (IsJumpKeyHeld())
            {
               // AttachHook2(m_Platform);
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

            OnPlayerHooked?.Invoke(hookTransform);
            
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
        m_CurrentHookState = EHookStates.SLIDING;
    }

    // Auto-Grab
    private void Sliding()
    {   
    
        DetachHook();

        if ( IsJumpKeyHeld())
        {
            m_CurrentHookState = EHookStates.HOOKING;
        }
        else
        {
            m_CurrentHookState = EHookStates.NONE;
        }
    }

    // Manual Grab
    private void Sliding2()
    {
        m_CurrentHookState = EHookStates.NONE;
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


    void PlayerSkinRotation()
    {
        float horizontal = m_PlayerInputService.m_Horizontal;
        float angle = horizontal * 90.0f;
        float targetRotationY = 0.0f;  

        if (IsJumpKeyHeld())
        {
            targetRotationY = isHooked ? 180.0f : angle * -1.0f;
        }
        else
        {
            targetRotationY = (isGrounded || (IsInAir() && !isHooked && horizontal != 0)) ? angle * -1.0f : 180.0f;
        }

        m_PlayerView.m_Skin.rotation = Quaternion.Euler(0, targetRotationY, 0);
}

    #region Terminal
    void TerminalCollided(bool isBonFire)
    {
        m_CurrentPlayerState = isBonFire ? EPlayerState.NORMAL : EPlayerState.ENTER_SLOWSTATE;

        SetPlayerState(isBonFire ? EPlayerState.NORMAL : EPlayerState.ENTER_SLOWSTATE);
    }


    void CheckPlayerState(EPlayerState state)
    {
        switch (state) 
        {
            case EPlayerState.NORMAL:
                StopFreezeTimer();
                break;

            case EPlayerState.ENTER_SLOWSTATE:
                StartFreezeTimer(m_PlayerConfig.m_FreezeTimer);
                break;

            case EPlayerState.SLOW_STATE:
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

       SetPlayerState(EPlayerState.SLOW_STATE);
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

  


    #endregion

    private float GetJumpSpeed()
    {
        if (m_CurrentPlayerState == EPlayerState.SLOW_STATE)
            return m_PlayerConfig.m_FreezeJump;

        return m_PlayerConfig.m_JumpSpeed;
    }

    private float GetMoveSpeed()
    {
        if (m_CurrentPlayerState == EPlayerState.SLOW_STATE)
            return m_PlayerConfig.m_FreezSpeed;

        return m_PlayerConfig.m_Speed;
    }
    private bool IsJumpKeyHeld()
    {
        return m_IsJumpKeyPressed;
    }
    private bool CheckIsGrounded()
    {
        isGrounded = Physics.Raycast(m_PlayerView.m_GroundCheckTransform.position, Vector3.down, m_PlayerConfig.m_GroundCheckDistance, m_PlayerConfig.m_WallLayer);

        return isGrounded;
    }
    private bool IsInAir()
    {
        return (!isGrounded);
    }

    private void SetMaterialColor(Color color)
    {
        m_PlayerView.m_Material.color = color;
    }
}
