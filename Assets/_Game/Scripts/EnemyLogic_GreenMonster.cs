using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic_GreenMonster : EnemyLogic
{
    private EWalkState m_WalkState;
    [SerializeField] private float m_Speed = 3;
    [SerializeField] private GameObject m_LeftMarker;
    [SerializeField] private GameObject m_RightMarker;

    protected override void InheritedStart()
    {
        base.InheritedStart();
        m_LeftMarker.transform.parent = transform.parent;
        m_RightMarker.transform.parent = transform.parent;
        m_WalkState = EWalkState.Left;
    }

    private void Update()
    {
        checkForPause();
        switch (m_WalkState)
        {
            case EWalkState.Left:
                turnLeft();
                transform.Translate(-m_Speed, 0, 0);
                break;
            case EWalkState.Idle:
                standStill();
                break;
            case EWalkState.Right:
                turnRight();
                transform.Translate(m_Speed, 0, 0);
                break;
            default:
                break;
        }
    }

    private void checkForPause()
    {
        switch (m_WalkState)
        {
            case EWalkState.Left:
                if(m_LeftMarker.transform.position.x > transform.position.x)
                {
                    StartCoroutine(DelayBeforeWalk(EWalkState.Right));
                }
                break;
            case EWalkState.Right:
                if (m_RightMarker.transform.position.x < transform.position.x)
                {
                    StartCoroutine(DelayBeforeWalk(EWalkState.Left));
                }
                break;
            default:
                break;
        }
    }

    IEnumerator DelayBeforeWalk(EWalkState newDirectionAfterPuase)
    {
        m_WalkState = EWalkState.Idle;
        yield return new WaitForSeconds(2);
        m_WalkState = newDirectionAfterPuase;
    }

    protected override void playDeathSound()
    {
        base.playDeathSound();
        SoundManager.GreenMonsterDeath();
    }
}
