using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthService : MonoBehaviour
{
    public static event Action<float> OnHealthChange;
    public static event Action<float> OnMaximumHealthSet;
 

    [SerializeField] private PlayerModel m_Config;
    [SerializeField] private float m_CurrrentBatteryValue;
    [SerializeField] private float m_MaxBatteryHealth { get { return m_Config.m_FreezeTimer; } }

    [SerializeField] private EPlayerState m_CurrentState;

    [SerializeField] private bool isTimerRun = false;

    private bool isBonFireEntered = false;

    private void Start()
    {

       SetBatteryHealth(m_MaxBatteryHealth);

       SetPlayerState(EPlayerState.ENTER_FREEZEING);
    }

    private void OnEnable()
    {
        BonFireTrigger.OnBonfireTriggered += OnBonfireCollision;
    }

    private void OnDisable()
    {
        BonFireTrigger.OnBonfireTriggered -= OnBonfireCollision;
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
                SetPlayerState(EPlayerState.FREEZE);
            }
        }
    }
    public void OnBatteryStateChange()
    {

      switch (m_CurrentState)
        {
            case EPlayerState.NORMAL:
                NormalState();
                break;
            case EPlayerState.ENTER_FREEZEING:
                EnteringFreezingState();
                break;

            case EPlayerState.FREEZE:
                FreezeState();
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

        OnBatteryStateChange();
    }

    private void NormalState()
    {
        isTimerRun = false;

        SetBatteryHealth(m_MaxBatteryHealth);
    }

    private void EnteringFreezingState()
    {
        isTimerRun = true;
    }

    private void FreezeState()
    {
        isTimerRun = false;

        SetBatteryHealth(0);
    }

    private void OnBonfireCollision(bool isInside)
    {
        isBonFireEntered = isInside;

        SetPlayerState(isBonFireEntered ? EPlayerState.NORMAL : EPlayerState.ENTER_FREEZEING);

    }


}
