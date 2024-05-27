using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerView : MonoBehaviour
{
   [Header("Dependencies")]
    public PlayerModel m_PlayerConfig;
    public PlayerInputService m_PlayerInputService;

    public Transform m_Tansform;
    public Collider m_Collider;
    public Rigidbody m_Rigidbody;
    public Material m_Material;
    public Animator m_Animator;
    public Transform m_Skin;

    [Header("Ground Check")]
    public Transform m_GroundCheckTransform;
    public LayerMask m_WallLayer;


    public bool isRight = false;

    private void OnEnable()
    {
        PlayerInputService.OnJumpPressed += JumpKeyPressed;
        PlayerInputService.OnJumpReleased += JumpKeyPressed;
    }
    private void OnDisable()
    {
       // PlayerInputService.OnJumpPressed -= JumpKeyPressed;
       // PlayerInputService.OnJumpReleased -= JumpKeyPressed;

    }

    private void Start()
    {

    }

    private void Update()
    {
       // HandleAnimations();

       // SetRightFacing();
       // SetMeshSkinRotaion();
    }

    void HandleAnimations()
    {
        //if (GetInputAxis().magnitude > 0.01f)
        //{

        //    m_Animator.Play(PlayerAnimationStrings.m_Run);
        //}
        //else
        //{
        //    m_Animator.Play(PlayerAnimationStrings.m_Idle);

        //}

        //if (isJump && !IsAnimationPlaying(PlayerAnimationStrings.m_JumpHang))
        //{
        //    m_Animator.Play(PlayerAnimationStrings.m_Idle);
        //}
    }


    private Vector2 GetInputAxis()
    {
        return m_PlayerInputService.moveVector;
    }


    private void SetRightFacing()
    {
        if (m_PlayerInputService.m_Horizontal == -1.0f)
        {
            isRight = false;
        }
        else if (m_PlayerInputService.m_Horizontal == 1.0f) 
        {
            isRight = true;
        }

    }

    void SetMeshSkinRotaion()
    {
        float targetAngle = !isRight ? -90 : 90;

        Quaternion turnRotation = Quaternion.Euler(0, targetAngle, 0);

      

        m_Skin.transform.rotation = Quaternion.Slerp(m_Skin.transform.rotation, 
            turnRotation, 20* Time.deltaTime);
    }

    public bool isJump = false;
    void JumpKeyPressed()
    {
      

        if (!IsAnimationPlaying(PlayerAnimationStrings.m_JumpHang))
        {
            m_Animator.Play(PlayerAnimationStrings.m_JumpHang);
            isJump = true;
        }

        // m_Animator.SetTrigger("Jump");

    }

    void JumpKeyReleased()
    {
        isJump = false;
        m_Animator.Play(PlayerAnimationStrings.m_Idle);

    }

    private bool IsAnimationPlaying(string animationName)
    {
        AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime < 1.0f;
    }

}
