﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] private Gun gunPrefab;
    [SerializeField] private Ball ballPrefab;

    private Queue<Ball> balls = new Queue<Ball>();
    private Gun gun;
    private Vector3 mainPosition;

    public int ball_count = 5;

    void Start()
    {
        GameManager.OnClick += Shooting;

        mainPosition = transform.position;
        gun = Instantiate(gunPrefab, mainPosition, Quaternion.identity);
        for(int i = 0; i < ball_count; i++)
        {
            Debug.Log(i);
            Ball ball = Instantiate(ballPrefab, mainPosition, Quaternion.identity);
            AddToQueue(ball);
        }
        
    }

    public bool CheckBallsCount() { return balls.Count == ball_count; } 

    public void FreezRotation() { gun.rotation = !gun.rotation;}

    public void InstantiateBall()
    {
        Ball ball = Instantiate(ballPrefab, mainPosition, Quaternion.identity);
        AddToQueue(ball);
    }

    public void AddToQueue(Ball ball)
    {
        ball.SetVisibility(false);
        ball.SetPosition(mainPosition);
        balls.Enqueue(ball);
    }
    
    public void Shooting()
    {
        StartCoroutine(WaitSecond());
    }

    IEnumerator WaitSecond()
    {
        int tempCount = balls.Count;
        for (int i = 0; i < tempCount; i++)
        {
            var ball = balls.Dequeue();
            ball.SetVisibility(true);
            gun.BallAddForce(ball);
            yield return new WaitForSeconds(.06f);
        }
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        Ball ball = coll.gameObject.GetComponent<Ball>();
        AddToQueue(ball);
    }

    void OnDestroy()
    {
        GameManager.OnClick -= Shooting;
    }
}