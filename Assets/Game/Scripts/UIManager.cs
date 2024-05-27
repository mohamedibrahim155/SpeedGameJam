using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public float m_Timer = 0;
    public TextMeshProUGUI m_TimerText;

    public bool m_TimerRun =false;


    public const string timerText = "Timer :";

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

    }

    private void Start()
    {
        m_TimerRun = true;
    }

    private void OnEnable()
    {
        GameManager.OnSceneLoaded += StopTimer;
    }
    private void OnDiosable()
    {
        GameManager.OnSceneLoaded -= StopTimer;

    }

    private void Update()
    {
        TimerUpdate();
    }

    void TimerUpdate()
    {
        if (m_TimerRun)
        {
            m_Timer += Time.deltaTime;

            if (m_TimerText != null)
            {
                m_TimerText.text = TimerText(m_Timer);
            }
        }
    }

    private string TimerText(float timer)
    {
        return timerText + timer.ToString("0.0");  
    }

    public void StopTimer()
    {
        m_TimerRun = false;
        Debug.Log(TimerText(m_Timer));
        ResetTimer();
    }
    public void ResetTimer()
    {
        m_Timer = 0;

    }

}
