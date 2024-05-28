using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public FxModel m_FxConfig;
    public Transform m_ParticlesHolder;
    public int MaxPrefabSpawnCount = 10;


   public struct ParticleData
    {
       public GameObject gameObject;
       public ParticleSystem particle;

        public bool IsParticlePlaying()
        {
            return particle.isPlaying;
        }

    }

    public int m_CurrentPoolIndex = 0;
    public Dictionary<int, ParticleData> listOfPoolHookedParticles = new Dictionary<int, ParticleData>();
    private void OnEnable()
    {
        EndPlatformTrigger.OnFxSpawn += SpawnFx;
        PlayerController.OnPlayerHooked += SpawnFromPool;
    }

    private void OnDisable()
    {
        EndPlatformTrigger.OnFxSpawn -= SpawnFx;
        PlayerController.OnPlayerHooked -= SpawnFromPool;


    }

    private void Start()
    {
        if (m_ParticlesHolder == null)
        {
            m_ParticlesHolder = this.transform;
        }
        Setup();
    }
    private void Setup()
    {
        for (int i = 0; i < MaxPrefabSpawnCount; i++)
        {
            GameObject prefab  =  GameObject.Instantiate(m_FxConfig.m_HookedFxPrefab, m_ParticlesHolder);
            prefab.SetActive(false);

            ParticleData data = new ParticleData();
            data.gameObject = prefab;
            data.particle = prefab.GetComponent<ParticleSystem>();

            AddHookedParticleInList(i, data);
        }
    }

    void AddHookedParticleInList(int index, ParticleData particle )
    {
        if (!listOfPoolHookedParticles.ContainsKey(index))
        {
            listOfPoolHookedParticles.Add(index, particle);
        }
    }

    public ParticleData GetParticleByIndex(int index)
    {
        return listOfPoolHookedParticles[index];
    }

    public void SpawnFromPool(Transform point)
    {
        ParticleData data = GetParticleByIndex(GetCurrentIndex());

       

        if (!data.IsParticlePlaying())
        {
            data.gameObject.SetActive(true);
            data.gameObject.transform.position = point.position;

            data.particle.Play();

            StartCoroutine(DisablePartilceOnComplete(data));
        }

        Debug.Log("On hook triggerred");
    }

    IEnumerator DisablePartilceOnComplete(ParticleData data)
    {
        yield return new WaitForSeconds(data.particle.main.duration);
        data.gameObject.SetActive(false);
        data.particle.Stop();
    }
   

    public int GetCurrentIndex()
    {
        if (m_CurrentPoolIndex >= listOfPoolHookedParticles.Count - 1) 
        {
            DeActvateEveryPartilce();
            m_CurrentPoolIndex = 0;

            return m_CurrentPoolIndex;
        }
        else
        {
            m_CurrentPoolIndex++;
        }

        return m_CurrentPoolIndex;
    }

    public void DeActvateEveryPartilce()
    {
        foreach (var item in listOfPoolHookedParticles)
        {
            item.Value.gameObject.SetActive(false);
            if (item.Value.IsParticlePlaying())
            { item.Value.particle.Stop(); }
        }
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
