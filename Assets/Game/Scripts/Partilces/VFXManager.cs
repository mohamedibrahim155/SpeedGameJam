using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public FxModel m_FxConfig;

    private void OnEnable()
    {
        EndPlatformTrigger.OnFxSpawn += SpawnFx;
    }

    private void OnDisable()
    {
        EndPlatformTrigger.OnFxSpawn -= SpawnFx;

    }


    private IEnumerator SpawnFxWithDelay(float delay, Transform point)
    {
        yield return new WaitForSeconds(delay);

        if (m_FxConfig != null && point !=null)
        {
            GameObject winParticle = GameObject.Instantiate(m_FxConfig.m_WinFxPrefab, point) as GameObject;

            Destroy(winParticle, 0.85f);
        }
       
    }
    private void SpawnFx(Transform spawnPoint)
    {
        StartCoroutine(SpawnFxWithDelay(1.0f, spawnPoint));
    }
}
