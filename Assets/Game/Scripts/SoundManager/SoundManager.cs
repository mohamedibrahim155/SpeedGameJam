using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{


    public SoundModel m_SoundConfig;
    public AudioSource m_JumpAuidoSource;


    public string m_BGMSoundTag = "BGM";
    private void OnEnable()
    {
        EndPlatformTrigger.OnFxSpawn += PlayLevelCompleteSFX;
        PlayerController.OnPlayerJumped += PlayJumpSound;

    }

    private void OnDisable()
    {
        EndPlatformTrigger.OnFxSpawn -= PlayLevelCompleteSFX;
        PlayerController.OnPlayerJumped -= PlayJumpSound;


    }

    private void Start()
    {
        CheckForBGMSpawned();
    }
    IEnumerator SpawnLevelCompleteSound(float duration,Transform point)
    {
        yield return new WaitForSeconds(duration);

        GameObject sound = GameObject.Instantiate(m_SoundConfig.m_LevelCompleteSFXPrefab, point) as GameObject;

        Destroy(sound, 1.0f);
    }
    void PlayLevelCompleteSFX(Transform spawnPostion)
    {
        StartCoroutine(SpawnLevelCompleteSound(1.0f, spawnPostion));
    }

    void PlayJumpSound()
    {
        if (m_JumpAuidoSource == null) return;

        if (!m_JumpAuidoSource.isPlaying)
        {
            m_JumpAuidoSource.Stop();
        } 
        m_JumpAuidoSource.Play();
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
