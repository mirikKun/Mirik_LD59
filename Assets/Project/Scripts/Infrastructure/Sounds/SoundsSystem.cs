using Project.Scripts.Infrastructure.Sounds.Enum;
using Project.Scripts.Infrastructure.Sounds.ScriptableObjects;
using Project.Scripts.Infrastructure.StaticData;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Project.Scripts.Infrastructure.Sounds
{
    public class SoundsSystem : ISoundsSystem
    {
        private const float LinearEpsilon = 0.0001f;

        private IStaticDataService _staticDataService;

        private GameObject _root;
        private AudioSource _music;
        private SoundManager _soundManager;

        private AudioMixer _mixer;
        private SoundMixersSO _mixers;

        private float _masterNormalized = 1f;
        private float _musicNormalized = 1f;
        private float _sfxNormalized = 1f;

        [Inject]
        private void Construct(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void SetMusicVolume(float volume)
        {
            _musicNormalized = Mathf.Clamp01(volume);
            if (EnsureMixer())
                ApplyMixerVolume(_mixers.MusicVolumeParameter, _musicNormalized);
        }

        public void SetSFXVolume(float volume)
        {
            _sfxNormalized = Mathf.Clamp01(volume);
            if (EnsureMixer())
                ApplyMixerVolume(_mixers.SfxVolumeParameter, _sfxNormalized);
        }

        public void SetMasterVolume(float volume)
        {
            _masterNormalized = Mathf.Clamp01(volume);
            if (EnsureMixer())
                ApplyMixerVolume(_mixers.MasterVolumeParameter, _masterNormalized);
        }

        public float GetMusicVolume() => _musicNormalized;

        public float GetSFXVolume() => _sfxNormalized;

        public float GetMasterVolume() => _masterNormalized;

        public SoundBuilder CreateSoundBuilder()
        {
            EnsureAudioSystem();
            return _soundManager != null ? _soundManager.CreateSoundBuilder() : null;
        }

        public void Play(SoundData soundData)
        {
            if (soundData == null)
                return;

            EnsureAudioSystem();
            if (_soundManager == null)
                return;

            _soundManager.CreateSoundBuilder().Play(soundData);
        }
        public void Play(DefaultSounds defaultSound)
        {

            if (_staticDataService.GetDefaultSoundsConfig().TryGet(defaultSound, out var soundData))
            {
                Play(soundData);
            }
        }


        public void PlayMusic(AudioClip clip, bool loop = true, float volumeScale = 1f)
        {
            if (clip == null)
                return;

            EnsureAudioSystem();
            _music.loop = loop;
            _music.clip = clip;
            _music.volume = Mathf.Clamp01(volumeScale);
            _music.Play();
        }

        public void StopMusic()
        {
            if (_music == null)
                return;

            _music.Stop();
            _music.clip = null;
        }

        private void ApplyMixerVolume(string parameterName, float normalizedVolume)
        {
            if (string.IsNullOrEmpty(parameterName) || _mixer == null || _mixers == null)
                return;

            normalizedVolume = Mathf.Clamp01(normalizedVolume);
            if (normalizedVolume <= LinearEpsilon)
            {
                _mixer.SetFloat(parameterName, _mixers.MinimalVolume);
                return;
            }

            float db = Mathf.Log10(normalizedVolume) * 20f;
            db = Mathf.Max(db, _mixers.MinimalVolume);
            _mixer.SetFloat(parameterName, db);
        }

        private bool EnsureMixer()
        {
            if (_mixer != null)
                return true;

            _mixers = _staticDataService.GetSoundMixersSo();
            if (_mixers == null || _mixers.MusicGroup == null)
                return false;

            _mixer = _mixers.MusicGroup.audioMixer;
            if (_mixer == null)
                return false;

            ApplyMixerVolume(_mixers.MasterVolumeParameter, _masterNormalized);
            ApplyMixerVolume(_mixers.MusicVolumeParameter, _musicNormalized);
            ApplyMixerVolume(_mixers.SfxVolumeParameter, _sfxNormalized);
            return true;
        }

        private void EnsureAudioSystem()
        {
            if (_root != null)
                return;

            if (!EnsureMixer())
                return;

            _root = new GameObject("[Sounds]");
            Object.DontDestroyOnLoad(_root);

            _music = _root.AddComponent<AudioSource>();
            _music.playOnAwake = false;
            _music.loop = true;
            _music.outputAudioMixerGroup = _mixers.MusicGroup;

            _soundManager = _root.AddComponent<SoundManager>();
            _soundManager.Construct(_mixers);
        }
    }
}
