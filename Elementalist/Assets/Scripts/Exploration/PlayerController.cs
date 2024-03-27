using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 MouseDelta;
    public Vector2 MoveComposite;

    public Transform MainCamera;
    
    private CharacterController _controller;

    public Vector3 _moveDirection;
    public float MoveSpeed;
    private float _lookDampening = 10f;

    private Animator _animator;

    private void Start()
    {
        MainCamera = Camera.main.transform;
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CalculateMoveDirection();
        LookMoveDirection();
        Move();
    }

    public void OnLook(InputValue lookDir)
    {
        MouseDelta = lookDir.Get<Vector2>();
    }

    public void OnMove(InputValue moveDir)
    {
        MoveComposite = moveDir.Get<Vector2>();
    }

    private void CalculateMoveDirection()
    {
        Vector3 cameraForward = new(MainCamera.forward.x, 0, MainCamera.forward.z);
        Vector3 cameraRight = new(MainCamera.right.x, 0, MainCamera.right.z);

        Vector3 moveDirection = cameraForward.normalized * MoveComposite.y + cameraRight.normalized * MoveComposite.x;

        _moveDirection.x = moveDirection.x * MoveSpeed;
        _moveDirection.z = moveDirection.z * MoveSpeed;
    }

    private void LookMoveDirection()
    {
        Vector3 lookDirection = new(_moveDirection.x, 0f, _moveDirection.z);

        if (lookDirection == Vector3.zero)
        {
            return;
        }
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), _lookDampening * Time.deltaTime);
    }

    private void Move()
    {
        _controller.Move(_moveDirection * Time.deltaTime);

        if (_moveDirection.x != 0 || _moveDirection.y != 0)
        {
            _animator.SetBool("Walking", true);
        }
        else
        {
            _animator.SetBool("Walking", false);
        }
    }
}
