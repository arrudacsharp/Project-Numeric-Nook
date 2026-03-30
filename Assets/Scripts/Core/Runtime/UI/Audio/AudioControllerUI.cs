using UnityEngine;
using NumericNook.Core.Runtime.Audio;
using TMPro;
using UnityEngine.UI;
using System;


namespace NumericNook.Core.Runtime.UI
{
    public class AudioControllerUI : MonoBehaviour
    {


        [SerializeField] private AudioController audioController;

        [SerializeField] private Slider musicSlider;
        [SerializeField] private Button musicMuteButton;
        [SerializeField] private TextMeshProUGUI musicMuteLabel;

        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Button sfxMuteButton;
        [SerializeField] TextMeshProUGUI sfxMuteLabel;

        [SerializeField] Sprite muteMusicDefaultIcon;
        [SerializeField] Sprite muteMusicMutedIcon;

        private void Start()
        {
            if (musicSlider == null || sfxSlider == null) return;

            musicSlider.value = audioController.MusicVolume;
            sfxSlider.value = audioController.SFXVolume;

            RefreshMusicMuteSprite();
            RefreshMuteLabels();


            musicSlider.onValueChanged.AddListener(audioController.SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(audioController.SetSFXVolume);


            if (musicMuteButton == null || sfxMuteButton == null) return;

            musicMuteButton.onClick.AddListener(OnMusicMutePressed);
            sfxMuteButton.onClick.AddListener(OnSFXMutePressed);

        }

        private void RefreshMusicMuteSprite()
        {
            if (musicMuteButton == null || audioController == null) return;
            musicMuteButton.image.sprite = audioController.MusicMuted ? muteMusicMutedIcon : muteMusicDefaultIcon;

        }

        private void OnMusicMutePressed()
        {
            audioController.ToggleMusicMute();
            musicMuteButton.image.sprite = audioController.MusicMuted ? muteMusicMutedIcon: muteMusicDefaultIcon;

            RefreshMuteLabels();
        }


        private void OnSFXMutePressed()
        {
            audioController.ToggleSFXMute();
            RefreshMuteLabels();
        }


        private void RefreshMuteLabels()
        {
            if (musicMuteLabel == null || sfxMuteLabel == null) return;

            musicMuteLabel.text = audioController.MusicMuted ? "Unmute" : "Mute";
            sfxMuteLabel.text = audioController.SFXMuted ? "Unmute" : "Mute";

        }

        private void OnDestroy()
        {
            musicSlider.onValueChanged.RemoveListener(audioController.SetMusicVolume);
            sfxSlider.onValueChanged.RemoveListener(audioController.SetSFXVolume );
        }

    }
}
