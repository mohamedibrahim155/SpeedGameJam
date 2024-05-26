using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthService : MonoBehaviour
{
    public static event Action OnHealthChange;

    public int m_CurrentHealth;
    public int m_MaxHealth;

    public int m_CurrentLives = 3;
    public int m_MaxLives = 3;

    private void Start()
    {
        
    }
    public void TakeDamage(int health)
    {
        m_CurrentHealth -= health;

        if (m_CurrentHealth <= 0) 
        {
            m_CurrentHealth = 0;
        }
        
        OnHealthChange?.Invoke();
    }


}
