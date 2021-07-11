using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] private int m_HealthPoints;
    protected Animator m_Animator;
    protected Rigidbody2D m_RigidBody;
    private bool m_IsFlying;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string collisionTag = collision.transform.tag;
        if(collisionTag == "PlayerAttack")
        {
            TakeDamage();
        }
        else if(collisionTag == "Player")
        {
            m_RigidBody.velocity = Vector2.zero;
        }
    }

    private void TakeDamage()
    {
        m_HealthPoints--;
        if(m_HealthPoints == 0)
        {
            Death();
        }
        else
        {
            m_Animator.SetTrigger("Hit");

        }
    }

    public void Death()
    {
        Destroy(gameObject, 1);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        m_RigidBody.bodyType = RigidbodyType2D.Kinematic;
        m_Animator.SetTrigger("Death");
        playDeathSound();
    }

    protected virtual void playDeathSound()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_RigidBody = GetComponent<Rigidbody2D>();
        InheritedStart();
    }

    protected virtual void InheritedStart()
    {

    }

    protected void turnLeft()
    {
        m_Animator.SetInteger("WalkDirection", -1);
    }

    protected void turnRight()
    {
        m_Animator.SetInteger("WalkDirection", 1);
    }

    protected void standStill()
    {
        m_Animator.SetInteger("WalkDirection", 0);
    }
}
