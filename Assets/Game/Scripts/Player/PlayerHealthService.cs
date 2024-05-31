using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthService : MonoBehaviour
{
    public static event Action<float> OnHealthChange;
    public static event Action<float> OnMaximumHealthSet;
 

    [SerializeField] private PlayerModel m_Config;
    [SerializeField] private float m_MaxBatteryHealth { get { return m_Config.m_FreezeTimer; } }
    [SerializeField] private bool isTimerRun = false;
    [SerializeField] private float m_CurrrentBatteryValue;
    [SerializeField] private EPlayerState m_CurrentState;


    private void Start()
    {

       SetBatteryHealth(m_MaxBatteryHealth);

       SetPlayerState(EPlayerState.ENTER_SLOWSTATE);
    }

    private void OnEnable()
    {
        TerminalTriggerBox.OnTerminalTriggered += TerminalTriggered;
    }

    private void OnDisable()
    {
        TerminalTriggerBox.OnTerminalTriggered -= TerminalTriggered;
    }


    private void Update()
    {
        TimerUpdate();
    }
    private void TimerUpdate()
    {
        if (isTimerRun)
        {
            if (m_CurrrentBatteryValue >= 0)
            {
                m_CurrrentBatteryValue -= Time.deltaTime;

                SetBatteryHealth(m_CurrrentBatteryValue);

            }
            else
            {
                SetPlayerState(EPlayerState.SLOW_STATE);
            }
        }
    }
    public void UpdatePlayerStates()
    {

      switch (m_CurrentState)
        {
            case EPlayerState.NORMAL:
                NormalState();
                break;
            case EPlayerState.ENTER_SLOWSTATE:
                EnterSlowState();
                break;

            case EPlayerState.SLOW_STATE:
                SlowState();
                break;

        }

    }



    void SetBatteryHealth(float batteryHealth)
    {
        m_CurrrentBatteryValue = batteryHealth;

        float fillamount = m_CurrrentBatteryValue / m_MaxBatteryHealth;

        OnHealthChange?.Invoke(fillamount);

    }

    public void SetPlayerState(EPlayerState state)
    {
        m_CurrentState = state;

        UpdatePlayerStates();
    }

    private void NormalState()
    {
        isTimerRun = false;

        SetBatteryHealth(m_MaxBatteryHealth);
    }

    private void EnterSlowState()
    {
        isTimerRun = true;
    }

    private void SlowState()
    {
        isTimerRun = false;

        SetBatteryHealth(0);
    }

    private void TerminalTriggered(bool isInside)
    {

        SetPlayerState(isInside ? EPlayerState.NORMAL : EPlayerState.ENTER_SLOWSTATE);

    }


}
