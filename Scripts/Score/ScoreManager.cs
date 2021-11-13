using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    public int m_score = 0;
    public int m_addScore = 0;
    int highScore = 0;

    bool changeScoreStanby = false;

    [SerializeField]float stanbyTime = 1.0f;
    float stanbyNowTime = 0.0f;

    [SerializeField] float animationTime = 1.0f;
    [SerializeField] TextMeshProUGUI[] scoreTexts;
    [SerializeField] TextMeshProUGUI[] highScoreTexts;
    [SerializeField] PlayerData playerData;

    private void Update()
    {
        UpdateScore();
        UpdateHighScore();
        
        if (changeScoreStanby)
        {
            StanbyCountUp();
        }
    }
    void UpdateScore()
    {
        if (scoreTexts != null)
        {
            foreach (var scoreText in scoreTexts)
            {
                scoreText.text = "Score:" + m_score;
            }
        }
    }

    void UpdateHighScore()
    {
        if (m_score > highScore)
        {
            highScore = m_score;
            if (highScoreTexts != null)
            {
                foreach (var highScoreText in highScoreTexts)
                {
                    highScoreText.text = "HighScore:" + highScore;
                }
            }
        }
    }

    void StanbyCountUp()
    {
        stanbyNowTime += Time.deltaTime;
        if (stanbyNowTime > stanbyTime)
        {
            ResetStanbyCount();
            changeScoreStanby = false;
            ChangeScore();
        }
    }

    public void AddScore(int addScore)
    {
        m_addScore += addScore;
        ResetStanbyCount();
        changeScoreStanby = true;
        //Debug.Log("addScore:" + addScore);
    }

    void ResetStanbyCount()
    {
        stanbyNowTime = 0;
    }

    public void ChangeScore()
    {
        DOTween.To(
            () => m_score,
            num => m_score = num,
            m_addScore,
            animationTime);
    }

    public void ResetScore()
    {
        m_addScore = 0;
        m_score = 0;
    }
}
