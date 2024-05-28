using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum EScene
{
    NONE = -1,
    MAIN_MENU = 0,
    TUTORIAL = 1,
    LEVEL_1 = 2,
    LEVEL_2 = 3,

}

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public static event Action OnSceneLoaded;
    public EScene m_CurrentScene;

    private Dictionary<int, Scene> listOfLevels = new Dictionary<int, Scene>();
    private void Awake()
    {


        if (Instance == null)
        {
            Instance = this;
          
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        IntializeAllScenes();
    }

    private void OnEnable()
    {
        EndPlatformTrigger.OnEndPlatformTrigger += OnLevelCompleted;
    }

    private void OnDisable()
    {
        EndPlatformTrigger.OnEndPlatformTrigger -= OnLevelCompleted;

    }

    #region SceneManagement
    void IntializeAllScenes()
    {

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            AddScene(i, SceneManager.GetSceneByBuildIndex(i));
           
        }

        m_CurrentScene = (EScene)SceneManager.GetActiveScene().buildIndex;

    }

    private void AddScene(int levelType, Scene levelName)
    {
        //if (!listOfLevels.ContainsKey(levelType))
        {
            listOfLevels.Add(levelType, levelName);
        }
    }



    public Scene GetScene(int index) 
    {
        return SceneManager.GetSceneByBuildIndex(index);
    }

    public Scene GetSceneByBuildIndex(string level)
    {
        return SceneManager.GetSceneByName(level);
    }

    public void LoadScene(int level)
    {
        SceneManager.LoadScene(level);
    }
 
    void OnLevelCompleted()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings) 
        {
            nextSceneIndex = 0;
        }


        StartCoroutine(DelayLoadNextLevel(nextSceneIndex));

    }
    public IEnumerator DelayLoadNextLevel(int level)
    {
        OnSceneLoaded?.Invoke();
        yield return new WaitForSeconds(2);
        
        LoadScene(level);
    }

    #endregion

}
