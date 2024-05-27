using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public FxModel m_FxConfig;

    private void OnEnable()
    {
        EndPlatformTrigger.OnFxSpanwn += SpawnFx;
    }

    private void OnDisable()
    {
        EndPlatformTrigger.OnFxSpanwn -= SpawnFx;

    }


    private IEnumerator SpawnFxWithDelay(float delay, Transform point)
    {
        yield return new WaitForSeconds(delay);

        GameObject winParticle = Instantiate(m_FxConfig.m_WinFxPrefab, point) as GameObject;

        Destroy(winParticle, 1);
    }
    private void SpawnFx(Transform spawnPoint)
    {
        StartCoroutine(SpawnFxWithDelay(1.0f, spawnPoint));
    }
}
