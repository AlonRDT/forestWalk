using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConclusionSceneSetup : MonoBehaviour
{
    private string m_PlayerPrefsScoreName = "PlayerScore";
    private string m_PlayerPrefsHighScoreName = "HighScore";
    private string m_PlayerPrefsIsVictoryName = "IsVictory";
    [SerializeField] private Text m_ScoreText;
    [SerializeField] private Text m_ConclusionText;
    [SerializeField] private Image m_BackgroundImage;

    // Start is called before the first frame update
    void Start()
    {
        int points = PlayerPrefs.GetInt(m_PlayerPrefsScoreName);
        PlayerPrefs.SetInt(m_PlayerPrefsScoreName, 0);
        if (PlayerPrefs.GetInt(m_PlayerPrefsIsVictoryName) == 0)
        {
            m_BackgroundImage.sprite = Resources.Load<Sprite>("Images/im_Loss");
            m_ConclusionText.text = $"<color=red>DEFEAT</color>";
            m_ScoreText.text = $"<color=red>Run Score: {points}\nBest Score: {PlayerPrefs.GetInt(m_PlayerPrefsHighScoreName)}</color>";
            SoundManager.DefeatMusic();
        }
        else
        {
            m_BackgroundImage.sprite = Resources.Load<Sprite>("Images/im_Victory");
            m_ConclusionText.text = $"<color=green>VICTORY</color>";
            m_ScoreText.text = $"<color=green>Run Score: {points}\nBest Score: {PlayerPrefs.GetInt(m_PlayerPrefsHighScoreName)}</color>";
            SoundManager.VictoryMusic();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("EntryScreen");

    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Level1");
    }


}
