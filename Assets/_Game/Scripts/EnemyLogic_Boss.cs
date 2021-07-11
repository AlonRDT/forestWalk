using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic_Boss : EnemyLogic
{
    [SerializeField] private GameObject m_LeftMarker;
    [SerializeField] private GameObject m_RightMarker;
    private bool m_IsReadyToJump;

    protected override void InheritedStart()
    {
        base.InheritedStart();
        m_LeftMarker.transform.parent = transform.parent;
        m_RightMarker.transform.parent = transform.parent;
        m_IsReadyToJump = true;
    }

    private void Update()
    {
        if (m_IsReadyToJump)
        {
            m_IsReadyToJump = false;

            if (m_LeftMarker.transform.position.x > transform.position.x)
            {
                StartCoroutine(Jump(ESide.Right));
            }
            else if (m_RightMarker.transform.position.x < transform.position.x)
            {
                StartCoroutine(Jump(ESide.Left));
            }
            else
            {
                int randomDirection = UnityEngine.Random.Range(0, 2);
                if (randomDirection == 0)
                {
                    StartCoroutine(Jump(ESide.Right));
                }
                else
                {
                    StartCoroutine(Jump(ESide.Left));
                }
            }
        }
    }

    IEnumerator Jump(ESide direction)
    {
        switch (direction)
        {
            case ESide.Left:
                m_RigidBody.AddForce(new Vector2(-300, 500), ForceMode2D.Impulse);
                turnLeft();
                break;
            case ESide.Right:
                m_RigidBody.AddForce(new Vector2(300, 500), ForceMode2D.Impulse);
                turnRight();
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(1);
        standStill();
        m_RigidBody.velocity = Vector2.zero;
        yield return new WaitForSeconds(2);
        m_IsReadyToJump = true;
    }

    protected override void playDeathSound()
    {
        base.playDeathSound();
        SoundManager.BossDeath();
    }
}
