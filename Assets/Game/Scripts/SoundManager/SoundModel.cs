using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "Assets/PlayerData")]
public class SoundModel : ScriptableObject
{
    public AudioClip m_BackgroundMusic;
    public AudioClip m_LevelCompleteSFX;

    public float volume;
}
