using System.Collections;
using UnityEngine;

namespace NumericNook.Core.Runtime.Audio
{
    public class AudioManager : MonoBehaviour
    {

        private static AudioManager instance;
        public static AudioManager Instance => instance;


        [SerializeField] private AudioSettings audioSettings;
        [SerializeField] private int sfxPoolSize = 4;
        [SerializeField] private AudioSource musicSourceA;
        [SerializeField] private AudioSource musicSourceB;

        private AudioSource[] sfxPool;
        private int sfxPoolIndex;
        private AudioSource activeMusicSource;
        private Coroutine crossFadeRoutine;


        private void Awake()
        {
            if (instance != null && instance != this)
            {

                Destroy(gameObject);
                return;

            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSettings.Load();
            BuildSFXPool();
            SetupMusicSources();

        }

        private void BuildSFXPool()
        {
            sfxPool= new AudioSource[sfxPoolSize];

            for (int i = 0; i < sfxPoolSize; i++)
            {
                GameObject go = new GameObject($"SFX_Source_{i}");
                go.transform.SetParent(transform);

                AudioSource source = go.AddComponent<AudioSource>();
                source.playOnAwake = false;
                sfxPool[i] = source;
            }
        }

        private void SetupMusicSources()
        {

            musicSourceA.loop = true;
            musicSourceA.playOnAwake = false;
            musicSourceB.loop = true;
            musicSourceB.playOnAwake = false;
            activeMusicSource = musicSourceA;

            ApplyMusicSettings();
            ApplySFXSettings();
        }


        public void ApplyMusicSettings()
        {
            float volume = audioSettings.musicMuted ? 0f : audioSettings.musicVolume;
            musicSourceA.volume = volume;
            musicSourceB.volume = volume;
        }

        public void ApplySFXSettings()
        {
            foreach (var source in sfxPool)
                source.volume = audioSettings.sfxMuted ? 0f : audioSettings.sfxVolume;
        }





        public void PlaySFX(AudioClip clip)
        {

            if (clip == null || audioSettings.sfxMuted) return;

            AudioSource source = sfxPool[sfxPoolIndex];
            source.clip = clip;
            source.volume = audioSettings.sfxVolume;
            source.Play();

            sfxPoolIndex = (sfxPoolIndex + 1) % sfxPoolSize;

        }

        public void PlayMusic(AudioClip clip)
        {
            if (clip == null) return;

            activeMusicSource.clip = clip;
            activeMusicSource.volume = audioSettings.musicMuted ? 0f : audioSettings.musicVolume;
            activeMusicSource.Play();
        }

        public void StopMusic()
        {
            if (crossFadeRoutine != null) StopCoroutine(crossFadeRoutine);
            activeMusicSource.Stop();
        }

        public void CrossFadeMusic(AudioClip clip, float duration = 1f)
        {
            if (clip == null) return;
            if (crossFadeRoutine != null) StopCoroutine(crossFadeRoutine);
            crossFadeRoutine = StartCoroutine(CrossfadeRoutine(clip, duration));
        }

        private IEnumerator CrossfadeRoutine(AudioClip clip, float duration)
        {

            AudioSource outgoing = activeMusicSource;
            AudioSource incoming = activeMusicSource == musicSourceA ? musicSourceB : musicSourceA;

            float targetVolume = audioSettings.musicMuted ? 0f : audioSettings.musicVolume;
            incoming.clip = clip;
            incoming.volume = 0f;
            incoming.Play();

            float elapsed = 0f;

            while (elapsed < duration)
            {

                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / duration;
                outgoing.volume = Mathf.Lerp(targetVolume, 0f, t);
                incoming.volume = Mathf.Lerp(0f, targetVolume, t);
                yield return null;


            }

            outgoing.Stop();
            outgoing.volume = targetVolume;
            activeMusicSource = incoming;
            crossFadeRoutine = null;
        }
    }
}
