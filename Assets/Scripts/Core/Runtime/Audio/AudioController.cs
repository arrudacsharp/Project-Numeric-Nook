using UnityEngine;

namespace NumericNook.Core.Runtime.Audio
{

    public class AudioController : MonoBehaviour
    {

        [SerializeField] AudioSettings audioSettings;

        public float MusicVolume => audioSettings.musicVolume;
        public float SFXVolume => audioSettings.sfxVolume;
        public bool MusicMuted => audioSettings.musicMuted;
        public bool SFXMuted => audioSettings.sfxMuted;



        public void SetMusicVolume(float value)
        {
            audioSettings.musicVolume = value;
            AudioManager.Instance.ApplyMusicSettings();
            audioSettings.Save();
        }


        public void SetSFXVolume(float value)
        {
            audioSettings.sfxVolume = value;
            AudioManager.Instance.ApplySFXSettings();
            audioSettings.Save();
        }

        public void ToggleMusicMute()
        {
            audioSettings.musicMuted = !audioSettings.musicMuted;
            AudioManager.Instance.ApplyMusicSettings();
            audioSettings.Save();

        }

        public void ToggleSFXMute()
        {
            audioSettings.sfxMuted = !audioSettings.sfxMuted;
            AudioManager.Instance.ApplySFXSettings();
            audioSettings.Save();
        }
    }
}
