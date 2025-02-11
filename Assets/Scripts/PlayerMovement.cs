using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public bool IsMoving => _isMoving;

    [SerializeField]
    private float Speed = 5.0f;

    private bool _isMoving;
    Rigidbody2D _rigidbody;

    private int score = 2000;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputValue value)
    {
        // Read value from control, the type depends on what
        // type of controls the action is bound to
        var inputVal = value.Get<Vector2>();

        Vector2 velocity = inputVal * Speed;
        _rigidbody.linearVelocity = velocity;

        _isMoving = (velocity.magnitude > 0.01f);
    }

    public void OnSaveScore()
    {
        // Usage example on how to save score
        PlayerPrefs.SetInt("Score", score);
        score = PlayerPrefs.GetInt("Score");
    }
}
