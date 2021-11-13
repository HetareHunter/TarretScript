using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    IGameStateChangeable gameStateChangeable;
    /// <summary>
    /// ゲーム時間単位は秒
    /// </summary>
    [SerializeField] float gameTime = 30.0f;
    float playNowTime = 0;
    [SerializeField] float toIdleStateTime = 4.0f;
    float currentToIdleTime = 0;
    [SerializeField] TextMeshProUGUI timeText;
    bool timeStart = false;
    bool gameEnd = false;
    private void Start()
    {
        playNowTime = gameTime;
        gameStateChangeable = GetComponent<IGameStateChangeable>();
    }
    // Update is called once per frame
    void Update()
    {
        if (timeStart)
        {
            PlayTimeCounter();
        }
        else if (gameEnd)
        {
            IdleTimeCounter();
        }
    }

    public void CountStart()
    {
        timeStart = true;
    }

    public void CountEnd()
    {
        timeStart = false;
        gameEnd = true;
        ResetTimer();
    }
    void PlayTimeCounter()
    {
        playNowTime -= Time.deltaTime;

        if (playNowTime <= 0)
        {
            gameStateChangeable.ToEnd();
        }
        timeText.text = "time:" + playNowTime.ToString("f2");
    }

    public void ResetTimer()
    {
        playNowTime = gameTime;
        timeText.text = "time:" + playNowTime.ToString("f2");
    }

    void IdleTimeCounter()
    {
        currentToIdleTime += Time.deltaTime;
        if (currentToIdleTime >= toIdleStateTime)
        {
            gameStateChangeable.ToIdle();
            gameEnd = false;
            currentToIdleTime = 0;
        }
    }
}
