
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    private int m_CurrentPoolIndex;
    private int maxPrefabCount { get { return m_MaxFxPrefabCount; } set { m_MaxFxPrefabCount = value; } }

    public FxModel m_FxConfig;
    public Transform m_ParticlesHolder;
    public int m_MaxFxPrefabCount = 10;
    public struct ParticleData
    {
       public GameObject gameObject;
       public ParticleSystem particle;

        public bool IsParticlePlaying()
        {
            return particle.isPlaying;
        }

    }
    public Dictionary<int, ParticleData> listOfPoolHookedParticles = new Dictionary<int, ParticleData>();

    private void OnEnable()
    {
        EndPlatformTrigger.OnFxSpawn += SpawnLevelCompleteFx;
        PlayerController.OnPlayerHooked += SpawnFromPool;
    }

    private void OnDisable()
    {
        EndPlatformTrigger.OnFxSpawn -= SpawnLevelCompleteFx;
        PlayerController.OnPlayerHooked -= SpawnFromPool;
    }

    private void Start()
    {
        Setup();
    }

    #region JumpFX
    private void Setup()
    {
        if (m_ParticlesHolder == null)
        {
            m_ParticlesHolder = this.transform;
        }

        for (int i = 0; i < maxPrefabCount; i++)
        {
            GameObject prefab  =  GameObject.Instantiate(m_FxConfig.m_HookedFxPrefab, m_ParticlesHolder);
            prefab.SetActive(false);

            ParticleData data = new ParticleData();
            data.gameObject = prefab;
            data.particle = prefab.GetComponent<ParticleSystem>();

            AddHookedParticleInList(i, data);
        }
    }

    private void AddHookedParticleInList(int index, ParticleData particle )
    {
        if (!listOfPoolHookedParticles.ContainsKey(index))
        {
            listOfPoolHookedParticles.Add(index, particle);
        }
    }

    private ParticleData GetParticleByIndex(int index)
    {
        return listOfPoolHookedParticles[index];
    }

    private void SpawnFromPool(Transform point)
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

    private IEnumerator DisablePartilceOnComplete(ParticleData data)
    {
        yield return new WaitForSeconds(data.particle.main.duration);
        data.gameObject.SetActive(false);
        data.particle.Stop();
    }


    private int GetCurrentIndex()
    {
        if (m_CurrentPoolIndex >= listOfPoolHookedParticles.Count - 1) 
        {
            DeActvateEveryParticle();
            m_CurrentPoolIndex = 0;

            return m_CurrentPoolIndex;
        }
        else
        {
            m_CurrentPoolIndex++;
        }

        return m_CurrentPoolIndex;
    }

    private void DeActvateEveryParticle()
    {
        foreach (var item in listOfPoolHookedParticles)
        {
            item.Value.gameObject.SetActive(false);
            if (item.Value.IsParticlePlaying())
            { item.Value.particle.Stop(); }
        }
    }
    #endregion


    #region Level_CompleteFx
    private IEnumerator SpawnLevelCompleteFxWithDelay(float delay, Transform point)
    {
        yield return new WaitForSeconds(delay);

        if (m_FxConfig != null && point !=null)
        {
            GameObject winParticle = GameObject.Instantiate(m_FxConfig.m_WinFxPrefab, point) as GameObject;

            Destroy(winParticle, 0.85f);
        }
       
    }
    private void SpawnLevelCompleteFx(Transform spawnPoint)
    {
        StartCoroutine(SpawnLevelCompleteFxWithDelay(1.0f, spawnPoint));
    }
    #endregion

}
