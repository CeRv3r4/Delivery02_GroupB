using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private float Speed = 5.0f;

    public float timeElapsed = 0f;
    public float score = 0f;
    public TextMeshProUGUI timeText; 
    public TextMeshProUGUI scoreText;

    Rigidbody2D _rigidbody;

    private Vector3 lastPosition; 

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        lastPosition = transform.position;
        score -= 2000;
    }

    public void Update()
    {
        timeElapsed += Time.deltaTime;

        float distanceTravelled = Vector3.Distance(lastPosition, transform.position);
        score += distanceTravelled; 

        lastPosition = transform.position;

        timeText.text = "Time: " + Mathf.Floor(timeElapsed).ToString(); 
        scoreText.text = "Score: " + Mathf.Floor(score).ToString(); 
    }

    public void OnMove(InputValue value)
    {
        var inputVal = value.Get<Vector2>();

        Vector2 velocity = inputVal * Speed;
        _rigidbody.linearVelocity = velocity;
    }

    public void SaveScoreAndLoadEnding()
    {
        PlayerPrefs.SetFloat("FinalScore", score); 
        PlayerPrefs.Save();
        SceneManager.LoadScene("ending");
    }
}
