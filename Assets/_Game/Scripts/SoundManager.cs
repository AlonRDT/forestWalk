using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static AudioSource m_AudioSource;
    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        //Debug.Log("4");

    }

    public static void PlayerHit()
    {
        m_AudioSource.PlayOneShot(Resources.Load<AudioClip>("Audio/ac_PlayerHit"));
    }

    public static void PlayerDeath()
    {
        m_AudioSource.PlayOneShot(Resources.Load<AudioClip>("Audio/ac_PlayerDeath"));

    }

    public static void CoinPickup()
    {
        m_AudioSource.PlayOneShot(Resources.Load<AudioClip>("Audio/ac_CoinPickup"));

    }

    public static void GreenMonsterDeath()
    {
        m_AudioSource.PlayOneShot(Resources.Load<AudioClip>("Audio/ac_GreenMonsterDeath"));

    }

    public static void FlierDeath()
    {
        m_AudioSource.PlayOneShot(Resources.Load<AudioClip>("Audio/ac_FlierDeath"));

    }

    public static void BossDeath()
    {
        m_AudioSource.PlayOneShot(Resources.Load<AudioClip>("Audio/ac_BossDeath"));

    }

    public static void LevelWin()
    {
        m_AudioSource.PlayOneShot(Resources.Load<AudioClip>("Audio/ac_WinLevel"));

    }

    public static void VictoryMusic()
    {
        m_AudioSource.clip = Resources.Load<AudioClip>("Audio/bm_Victory");
        m_AudioSource.loop = true;
        m_AudioSource.Play();
    }

    public static void DefeatMusic()
    {
        m_AudioSource.clip = Resources.Load<AudioClip>("Audio/bm_Defeat");
        m_AudioSource.loop = true;
        m_AudioSource.Play();
    }
}
