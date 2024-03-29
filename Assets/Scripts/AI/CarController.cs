﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField] private float power = 5;
    [SerializeField] private float torque = 0.5f;
    [SerializeField] private float maxSpeed = 5;

    [SerializeField] private Vector2 _movementVector;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector2 moveInput)
    {
        this._movementVector = moveInput;
    }

    private void FixedUpdate()
    {
        if(_rb.velocity.magnitude < maxSpeed)
        {
            _rb.AddForce(_movementVector.y * transform.forward * power);
        }

        _rb.AddTorque(_movementVector.x * Vector3.up * torque * _movementVector.y);
    }
}
