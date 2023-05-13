using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]public CharacterController _controller;
    public Transform cam;

    public float normalSpeed = 6f;
    public float sprintSpeed = 8f;
    public float jumpHeight = 4f;

    private float _currentSpeed;

    Animator animator;

    private Vector2 _input;
    private Vector3 _direction;
    private Vector3 _moveDir;

    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 1.0f;
    private float _velocity;

    bool isRunning = false;

    public float turnDamp = 0.1f;
    float turnDampVelocity;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        animator = this.GetComponent<Animator>();
        _currentSpeed = normalSpeed;
        _controller = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        applyGravity();
        applyRotation();
        applyMovement();
    }

    void applyGravity()
    {
        if (IsGrounded() && _velocity < 0.0f)
            _velocity = -1.0f;
        else
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;

        _direction.y = _velocity;
    }

    void applyRotation()
    {
        if (_input.sqrMagnitude == 0)
            return;

        float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnDampVelocity, turnDamp);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        _moveDir = Quaternion.Euler(0f, targetAngle, 0f) * new Vector3(0, _velocity, 1);
    }

    void applyMovement()
    {
        isRunning = false;
        animator.SetBool("isRunning", isRunning);

        Debug.Log(_currentSpeed);

        if (_input.magnitude >= 0.1f)
        {
            isRunning = true;
            animator.SetBool("isRunning", isRunning);
        }


        _controller.Move(_moveDir.normalized * _currentSpeed * Time.deltaTime);
    }

    public void move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0f, _input.y).normalized;
    }

    public void jump(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;
        if (!IsGrounded())
            return;

        _velocity += jumpHeight;
    }

    public void sprint(InputAction.CallbackContext context)
    {
        if (!context.ReadValueAsButton())
        {
            _currentSpeed = normalSpeed;
            return;
        }   
        _currentSpeed = sprintSpeed;
    }

    private bool IsGrounded() => _controller.isGrounded;
}
