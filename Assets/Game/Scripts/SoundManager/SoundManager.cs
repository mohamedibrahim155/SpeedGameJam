using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public SoundModel m_SoundConfig;

    public string m_BGMSoundTag = "BGM";
    private void OnEnable()
    {
        EndPlatformTrigger.OnFxSpawn += PlayLevelCompleteSFX;
    }

    private void OnDisable()
    {
        EndPlatformTrigger.OnFxSpawn -= PlayLevelCompleteSFX;
    }

    private void Start()
    {
        CheckForBGMSpawned();
    }
    IEnumerator SpawnSound(float duration,Transform point)
    {
        yield return new WaitForSeconds(duration);

        GameObject sound = GameObject.Instantiate(m_SoundConfig.m_LevelCompleteSFXPrefab, point) as GameObject;

        Destroy(sound, 1.0f);
    }
    void PlayLevelCompleteSFX(Transform spawnPostion)
    {
        StartCoroutine(SpawnSound(1.0f, spawnPostion));
    }


    private void CheckForBGMSpawned()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(m_BGMSoundTag);

        if (obj == null)
        {
            obj = GameObject.Instantiate(m_SoundConfig.m_BackgroundMusicPrfab);
            DontDestroyOnLoad(obj);
        }


    }
}
