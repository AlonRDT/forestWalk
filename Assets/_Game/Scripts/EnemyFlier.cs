using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlier : EnemyLogic
{
    private Vector3 m_StartLocation;
    private bool m_IsChasing;
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_MaxSpeed;
    [SerializeField] private float m_EngageDistance;
    [SerializeField] private bool m_Debug;

    // Start is called before the first frame update
    protected override void InheritedStart()
    {
        base.InheritedStart();
        m_StartLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerDirection = getDistanceAndTurn(PlayerInput.Player.transform.position, transform.position);
        if (m_Debug) 
            Debug.Log(playerDirection.magnitude);
        if (playerDirection.magnitude < m_EngageDistance)
        {
            m_RigidBody.AddForce(playerDirection * m_Speed * Time.deltaTime, ForceMode2D.Impulse);
            m_IsChasing = true;
        }
        else
        {
            if (m_IsChasing)
            {
                m_RigidBody.velocity = Vector2.zero;
            }
            Vector3 startDirection = getDistanceAndTurn(m_StartLocation, transform.position);
            if (startDirection.magnitude > 0.1f)
            {
                
                m_RigidBody.AddForce(startDirection * m_Speed * Time.deltaTime * determineMoveDirection(startDirection), ForceMode2D.Impulse);
                m_IsChasing = false;
            }
            else
            {
                m_RigidBody.velocity = Vector2.zero;
            }
        }
        m_RigidBody.velocity = new Vector2(Mathf.Clamp(m_RigidBody.velocity.x, -m_MaxSpeed, m_MaxSpeed), Mathf.Clamp(m_RigidBody.velocity.y, -m_MaxSpeed, m_MaxSpeed));
    }

    private int determineMoveDirection(Vector3 distance)
    {
        int output = 1; 
        float upperFraction = Mathf.Pow(m_RigidBody.velocity.magnitude, 2);
        float lowerFraction = m_Speed * 2;
        float stoppingDistance = upperFraction / lowerFraction;
        if(stoppingDistance > distance.magnitude)
        {
            output = -1;
        }

        return output;
    }

    private Vector3 getDistanceAndTurn(Vector3 targetLocation, Vector3 currentLocation)
    {
        Vector3 direction = targetLocation - currentLocation;
        if (direction.magnitude < 0.1f)
        {
            standStill();
        }
        else if (targetLocation.x > currentLocation.x)
        {
            turnRight();
        }
        else
        {
            turnLeft();
        }
        return direction;
    }

    protected override void playDeathSound()
    {
        base.playDeathSound();
        SoundManager.FlierDeath();
    }
}
