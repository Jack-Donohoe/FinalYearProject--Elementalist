using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public CharacterController controller;
    
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveDir = context.ReadValue<Vector2>();
        controller.Move(new Vector3(moveDir.x, 0, moveDir.y).normalized);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        controller.Move(new Vector3(0, 10, 0));
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        
    }
}
