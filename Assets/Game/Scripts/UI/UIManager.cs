using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public const string TIMER_TEXT = "Timer : ";
    public const string BEST_TIMER_TEXT = "Best : ";

    [Header("UI References")]
    public TextMeshProUGUI m_TimerText;
    public TextMeshProUGUI m_BestTimerText;
    public Slider m_BatterySlider;
    public GameObject m_PauseMenuCanvas;
    public GameObject m_GameCompleteCanvas;

    public bool m_TimerRun =false;
    public bool m_IsMainMenu = false;
    public float m_Timer = 0;



    [SerializeField] private EScene m_CurrentLevel;
    [SerializeField] private float m_CurrentHighScore;

    private bool isGamePaused;

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

        m_CurrentLevel = (EScene)SceneManager.GetActiveScene().buildIndex;
        SetHighScore();
    }

  

    private void OnEnable()
    {
        GameManager.OnSceneLoaded += SaveHighScoreOnLoad;
        GameManager.OnGameComplete += GameCompletePopUp;
        PlayerInputService.OnEscapePressed += OnEscapeKeyPressed;
        PlayerHealthService.OnHealthChange += BatteryHealthBarChange;


    }
    private void OnDisable()
    {
        GameManager.OnSceneLoaded -= SaveHighScoreOnLoad;
        GameManager.OnGameComplete -= GameCompletePopUp;

        PlayerInputService.OnEscapePressed -= OnEscapeKeyPressed;
        PlayerHealthService.OnHealthChange -= BatteryHealthBarChange;


    }

    private void Update()
    {
        if (!m_IsMainMenu)
        {
            TimerUpdate(m_CurrentLevel);
        }
    }

    private void SetHighScore()
    {
        m_CurrentHighScore = GetHighScoreBasedOnLevel(m_CurrentLevel);

        if (m_BestTimerText != null)
        {
            m_BestTimerText.text = (m_CurrentHighScore == -1) ? m_BestTimerText.text :
                TimerText(BEST_TIMER_TEXT, m_CurrentHighScore);
        }
    }
    void TimerUpdate(EScene level)
    {
        if (level == EScene.LEVEL_1 || level == EScene.LEVEL_2)
        {
            if (m_TimerRun)
            {
                m_Timer += Time.deltaTime;

                if (m_TimerText != null)
                {
                    m_TimerText.text = TimerText(TIMER_TEXT, m_Timer);
                }
                
            }
        }
        
    }

    private string TimerText(string appendString,float timer)
    {
        return appendString + FormatTimer(timer);  
    }

    public void SaveHighScoreOnLoad()
    {
        if(m_TimerRun) 
        {
            SaveBestHighScoreTimer(m_Timer);
        } 

        ResetTimer();
    }
    private string FormatTimer(float timer)
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        int hundredths = Mathf.FloorToInt((timer * 100f) % 100f);
        return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, hundredths);
        //return string.Format("<color=#4D4DFF>{0:00}</color>:<color=#00FF00>{1:00}</color>.<color=#00FF00>{2:00}</color>", minutes, seconds, hundredths);
    }

    public void ResetTimer()
    {
        m_TimerRun = false;
        m_Timer = 0;
    }


    public void QuitButton()
    {
        Debug.Log("Quit Pressed");
        Application.Quit();
    }

    public void LoadScene(int sceneIndex)
    {
        GameManager.Instance.LoadScene(sceneIndex);
    }

    public void SaveBestHighScoreTimer(float newTime)
    {
        string bestTimerBaseByLevel = GetLevelString(m_CurrentLevel);

        if(string.IsNullOrEmpty(bestTimerBaseByLevel)) 
        {
            Debug.Log(" Save best Timer is  NUll");
            return;
        }
        float savedHighScore = PlayerPrefs.GetFloat(bestTimerBaseByLevel, float.MaxValue);

        if (newTime < savedHighScore) 
        {
            PlayerPrefs.SetFloat(bestTimerBaseByLevel, newTime);
        }

    }

    public float GetHighScoreBasedOnLevel(EScene eLevel)
    {
        float highScore = PlayerPrefs.GetFloat(GetLevelString(eLevel), -1.0f);
        return highScore;
    }
    private string GetLevelString(EScene eLevel)
    {
        switch (eLevel)
        {
            case EScene.LEVEL_1: return "HighScore_Level_1";
            case EScene.LEVEL_2: return "HighScore_Level_2";
        }
        return " ";
    }

    private void OnEscapeKeyPressed()
    {
        if (m_IsMainMenu) return;

        isGamePaused = !isGamePaused;


        if (m_PauseMenuCanvas != null)
        {
            m_PauseMenuCanvas.SetActive(isGamePaused);

            if (isGamePaused)
            {
                PauseMenu();
            }
            else
            {
                Resume();
            }
        }


    }



    public void PauseMenu()
    {
        Time.timeScale = 0.0f;
    }

    public void Resume()
    {
        if (isGamePaused) { isGamePaused = false; }

        Time.timeScale = 1.0f;
    }

    void BatteryHealthBarChange(float batteryHealth)
    {
        if (m_BatterySlider == null) return;

        m_BatterySlider.value = batteryHealth;

    }

    void GameCompletePopUp()
    {
        if (m_GameCompleteCanvas == null) return;

        SaveHighScoreOnLoad();

        PauseMenu();

        m_GameCompleteCanvas.SetActive(true);

        GameOverPopUp gamepopUp = m_GameCompleteCanvas.GetComponent<GameOverPopUp>();

        ShowHighscore(gamepopUp.m_Level1, GetHighScoreBasedOnLevel(EScene.LEVEL_1));
        ShowHighscore(gamepopUp.m_Level2, GetHighScoreBasedOnLevel(EScene.LEVEL_2));
    }

    void ShowHighscore(TextMeshProUGUI textUi, float value)
    {
        textUi.text = FormatTimer(value);
    }

   


}
