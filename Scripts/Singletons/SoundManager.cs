using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    #region Singleton
    public static SoundManager SOUND_MANAGER;
    #endregion
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource buttonsAudioSource;
    [SerializeField] private AudioSource backgroundMusicAudioSource;
    [SerializeField] private Sprite soundEffectUnmuteSprite;
    [SerializeField] private Sprite soundEffectMuteSprite;
    [SerializeField] private Sprite backgroundMusicUnmuteSprite;
    [SerializeField] private Sprite backgroundMusicMuteSprite;
    [SerializeField] private Image backgroundMusicImage;
    [SerializeField] private Image soundEffectImage;
    private bool areSoundEffectsMute = false;
    private void Awake()
    {
        SOUND_MANAGER = this;
    }
    public void PlaySound(AudioClip sound, float soundVolume)
    {
        audioSource.volume = soundVolume;
        audioSource.PlayOneShot(sound);
    }
    public void PlayButtonsSound(AudioClip sound, float soundVolume)
    {
        buttonsAudioSource.volume = soundVolume;
        buttonsAudioSource.PlayOneShot(sound);
    }
    public AudioSource GetAudioSource()
    {
        return audioSource;
    }
    public AudioSource GetButtonsAudioSource()
    {
        return buttonsAudioSource;
    }
    public void BackgroundMusicStatus()
    {
        if (backgroundMusicAudioSource.mute)
        {
            backgroundMusicAudioSource.mute = false;
            backgroundMusicImage.sprite = backgroundMusicUnmuteSprite;
        }
        else
        {
            backgroundMusicAudioSource.mute = true;
            backgroundMusicImage.sprite = backgroundMusicMuteSprite;
        }
    }
    public void BackgroundMusicStatus(bool mute)
    {
        if(mute)
        {
            backgroundMusicAudioSource.mute = true;
            backgroundMusicImage.sprite = backgroundMusicMuteSprite;
        }
        else
        {
            backgroundMusicAudioSource.mute = false;
            backgroundMusicImage.sprite = backgroundMusicUnmuteSprite;
        }
    }
    public void SoundEffectStatus()
    {
        if (SOUND_MANAGER.GetAudioSource().mute)
        {
            areSoundEffectsMute = false;
            GameObject.FindObjectsOfType<AudioSource>()
                .Where(source => !source.name.Equals("Background Music"))
                .ToList()
                .ForEach(source => source.mute = false);
            SOUND_MANAGER.GetButtonsAudioSource().mute = false;
            soundEffectImage.sprite = soundEffectUnmuteSprite;
        }
        else
        {
            areSoundEffectsMute = true;
            GameObject.FindObjectsOfType<AudioSource>()
                .Where(source => !source.name.Equals("Background Music"))
                .ToList()
                .ForEach(source => source.mute = true);
            SOUND_MANAGER.GetButtonsAudioSource().mute = true;
            soundEffectImage.sprite = soundEffectMuteSprite;
        }
    }
    public bool GetSoundEffectsStatus()
    {
        return areSoundEffectsMute;
    }
}
