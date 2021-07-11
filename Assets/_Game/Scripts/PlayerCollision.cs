using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerCollision : MonoBehaviour
{
    private int m_HealthPoints = 3;
    private string m_PlayerPrefsScoreName = "PlayerScore";
    private string m_PlayerPrefsHighScoreName = "HighScore";
    private string m_PlayerPrefsIsVictoryName = "IsVictory";
    private int m_PlayerLayerIndex, m_EnemyLayerIndex;
    [SerializeField] private Text m_ScoreText;
    [SerializeField] private GameObject m_AttackLeftCollider;
    [SerializeField] private GameObject m_AttackRightCollider;
    [SerializeField] private GameObject m_BlockLeftCollider;
    [SerializeField] private GameObject m_BlockRightCollider;
    [SerializeField] private GameObject m_CoinPickupEffect;
    public bool IsDead { get; set; }
    public bool IsStunned { get; set; }
    private float m_DelayToNotStunned = 0.25f;
    private Animator m_Animator;
    private Rigidbody2D m_RigidBody;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_PlayerLayerIndex = LayerMask.NameToLayer("UI");
        m_EnemyLayerIndex = LayerMask.NameToLayer("Water");
        PlayerPrefs.SetInt(m_PlayerPrefsScoreName, 0);
        updateText();
        EnableEnemyCollisoin();
    }

    public void DisableEnemyCollisoin()
    {
        Physics2D.IgnoreLayerCollision(m_PlayerLayerIndex, m_EnemyLayerIndex, true);
    }

    public void EnableEnemyCollisoin()
    {
        Physics2D.IgnoreLayerCollision(m_PlayerLayerIndex, m_EnemyLayerIndex, false);
    }

    public void AttackLeft(float waitTime)
    {
        StartCoroutine(Attack(m_AttackLeftCollider, waitTime));
    }

    public void AttackRight(float waitTime)
    {
        StartCoroutine(Attack(m_AttackRightCollider, waitTime));
    }

    IEnumerator Attack(GameObject targetCollider, float waitTime)
    {
        yield return new WaitForSeconds(waitTime / 2);
        targetCollider.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        targetCollider.SetActive(false);
    }

    public void BlockLeft()
    {
        m_BlockLeftCollider.SetActive(true);
    }

    public void BlockRight()
    {
        m_BlockRightCollider.SetActive(true);
    }

    public void UnblockLeft()
    {
        m_BlockLeftCollider.SetActive(false);
    }

    public void UnblockRight()
    {
        m_BlockRightCollider.SetActive(false);
    }

    private void takeDamage(Transform collisionTransform)
    {
        m_HealthPoints--;
        if (m_HealthPoints == 0)
        {
            death();
        }
        else
        {
            SoundManager.PlayerHit();
            m_Animator.SetTrigger("Hurt");
            StartCoroutine(Stunned());
            pushPlayer(determineSide(collisionTransform));
            updateText();
        }
    }

    private void pushPlayer(ESide direction)
    {
        float xForce = 5;

        if (direction == ESide.Left)
            xForce *= -1;

        m_RigidBody.AddForce(new Vector2(xForce, 0), ForceMode2D.Impulse);
    }

    IEnumerator Stunned()
    {
        IsStunned = true;
        DisableEnemyCollisoin();
        yield return new WaitForSeconds(m_DelayToNotStunned);
        IsStunned = false;
        yield return new WaitForSeconds(m_DelayToNotStunned * 2);
        EnableEnemyCollisoin();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string collisionTag = collision.transform.tag;
        if (collisionTag == "Coin")
        {
            Vector3 contactPointfloat = collision.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position);


            if (handleMoneyPickup(collision.GetComponent<Tilemap>(), contactPointfloat))
            {
                victory();
            }
        }
        else if (collisionTag == "Death")
        {
            death();
        }
    }

    private void victory()
    {
        PlayerPrefs.SetInt(m_PlayerPrefsIsVictoryName, 1);
        DisableEnemyCollisoin();
        SoundManager.LevelWin();
        StartCoroutine(EndLevel());
    }

    IEnumerator EndLevel()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Conclusion");
    }

    private void death()
    {
        m_HealthPoints = 0;
        IsDead = true;
        m_Animator.SetBool("noBlood", false);
        m_Animator.SetTrigger("Death");
        PlayerPrefs.SetInt(m_PlayerPrefsIsVictoryName, 0);
        DisableEnemyCollisoin();
        updateText();
        SoundManager.PlayerDeath();
        StopAllCoroutines();
        StartCoroutine(EndLevel());
    }

    private bool checkVictory(Tilemap collectables)
    {
        bool output = true;

        foreach (var pos in collectables.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = collectables.CellToWorld(localPlace);
            if (collectables.HasTile(localPlace))
            {
                output = false;
                break;
            }
        }

        return output;
    }

    private void updateText()
    {
        if (PlayerPrefs.GetInt(m_PlayerPrefsScoreName) > PlayerPrefs.GetInt(m_PlayerPrefsHighScoreName))
        {
            PlayerPrefs.SetInt(m_PlayerPrefsHighScoreName, PlayerPrefs.GetInt(m_PlayerPrefsScoreName));
            //Debug.Log(PlayerPrefs.GetInt(m_PlayerPrefsHighScoreName));
        }
        m_ScoreText.text = $"<color=yellow>Score: {PlayerPrefs.GetInt(m_PlayerPrefsScoreName)}</color>\n<color=Red>Health: {m_HealthPoints}</color>";
    }

    private bool handleMoneyPickup(Tilemap collectables, Vector3 contactPointfloat)
    {
        Vector3Int contactPoint1 = new Vector3Int(Mathf.RoundToInt(contactPointfloat.x), Mathf.RoundToInt(contactPointfloat.y), Mathf.RoundToInt(contactPointfloat.z));
        Vector3Int contactPoint2 = new Vector3Int(Mathf.FloorToInt(contactPointfloat.x), Mathf.FloorToInt(contactPointfloat.y), Mathf.RoundToInt(contactPointfloat.z));
        Vector3Int contactPoint3 = new Vector3Int(Mathf.RoundToInt(contactPointfloat.x), Mathf.FloorToInt(contactPointfloat.y), Mathf.RoundToInt(contactPointfloat.z));
        Vector3Int contactPoint4 = new Vector3Int(Mathf.FloorToInt(contactPointfloat.x), Mathf.RoundToInt(contactPointfloat.y), Mathf.RoundToInt(contactPointfloat.z));
        if (collectables.HasTile(contactPoint1))
        {
            emptyTileIncreaseScore(collectables, contactPoint1);
        }
        if (collectables.HasTile(contactPoint2))
        {
            emptyTileIncreaseScore(collectables, contactPoint2);
        }
        if (collectables.HasTile(contactPoint3))
        {
            emptyTileIncreaseScore(collectables, contactPoint3);
        }
        if (collectables.HasTile(contactPoint4))
        {
            emptyTileIncreaseScore(collectables, contactPoint4);
        }
        updateText();

        return checkVictory(collectables);
    }

    private void emptyTileIncreaseScore(Tilemap collectables, Vector3Int contactPoint)
    {
        PlayerPrefs.SetInt(m_PlayerPrefsScoreName, PlayerPrefs.GetInt(m_PlayerPrefsScoreName) + 10);
        SoundManager.CoinPickup();
        GameObject pickupEffect = Instantiate(m_CoinPickupEffect, collectables.GetCellCenterWorld(contactPoint), Quaternion.identity);
        collectables.SetTile(contactPoint, null);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string collisionTag = collision.transform.tag;
        
        if (collisionTag == "Enemy")
        {
            takeDamage(collision.transform);
        }
    }

    private ESide determineSide(Transform EnemyTransform)
    {
        ESide output = ESide.Left;

        if (transform.position.x > EnemyTransform.position.x)
            output = ESide.Right;

        return output;
    }
}
