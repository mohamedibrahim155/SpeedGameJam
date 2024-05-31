using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SoundData", menuName = "Assets/SoundData")]
public class SoundModel : ScriptableObject
{
    public GameObject m_BackgroundMusicPrfab;
    public GameObject m_LevelCompleteSFXPrefab;
    public GameObject m_JumpSFXPrefab;
}
