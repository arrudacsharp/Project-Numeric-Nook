using UnityEngine;

namespace NumericNook.Core.Runtime.Audio
{
    [CreateAssetMenu(fileName = "AudioSettings", menuName = "NumberNook/Audio/AudioSettings")]
    public class AudioSettings : ScriptableObject
    {

        [Header("Music")]
        [Range(0f, 1f)] public float musicVolume = 1f;
        public bool musicMuted = false;

        [Header("SFX")]
        [Range(0f, 1f)] public float sfxVolume = 1f;
        public bool sfxMuted = false;

        public void Save()
        {
            PlayerPrefs.SetFloat("musicVolume", musicVolume);
            PlayerPrefs.SetInt("musicMuted", musicMuted ? 1 : 0);
            PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
            PlayerPrefs.SetInt("sfxMuted", sfxMuted ? 1 : 0);
            PlayerPrefs.Save();
        }

        public void Load()
        {

            musicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
            musicMuted = PlayerPrefs.GetInt("musicMuted", 0) == 1;
            sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);
            sfxMuted = PlayerPrefs.GetInt("sfxMuted", 0) == 1;
        }
    }
}
