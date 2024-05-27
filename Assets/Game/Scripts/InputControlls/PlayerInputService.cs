using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputService : MonoBehaviour
{
    public static event  Action OnJumpPressed;
    public static event  Action OnJumpReleased;
    public static event Action OnJumpHold;

    private PlayerControls playerControls;
    public Vector2 moveVector;


    public float m_Horizontal;
    public float m_Vertical;



    private void OnEnable()
    {
        playerControls = new PlayerControls();

        playerControls.Player.Enable();
        playerControls.Player.Move.Enable();

        playerControls.Player.Move.performed += mvoeVector => InputRead();

        playerControls.Player.Jump.Enable();
        playerControls.Player.Jump.performed += jumpPressed => OnJumpPressed?.Invoke();
        playerControls.Player.Jump.canceled += jumpPressed => OnJumpReleased?.Invoke();

       // playerControls.Player.Jump.in += jumpPressed => OnJumpPressed?.Invoke();
        
    }



    private void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.Player.Move.Disable();
            playerControls.Player.Jump.Disable();
        }


    }
    void InputRead()
    {
        moveVector = playerControls.Player.Move.ReadValue<Vector2>();

        m_Horizontal = moveVector.x;
        m_Vertical = moveVector.y;
    }


}

    
