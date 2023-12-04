using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, PlayerInputActions.IPlayerActions
{
    public Vector2 MouseLook;
    public Vector2 MoveDirection;

    public Action OnJumpPerformed;

    private PlayerInputActions inputs;

    private void OnEnable()
    {
        if (inputs != null)
            return;

        inputs = new PlayerInputActions();
        inputs.Player.SetCallbacks(this);
        inputs.Player.Enable();
    }

    public void OnDisable()
    {
        inputs.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveDirection = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnJumpPerformed?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        MouseLook = context.ReadValue<Vector2>();
    }
}