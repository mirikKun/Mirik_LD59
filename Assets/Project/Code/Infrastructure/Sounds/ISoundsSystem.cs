using Code.Infrastructure.Sounds.Enum;
using UnityEngine;

namespace Code.Infrastructure.Sounds
{
    public interface ISoundsSystem
    {
        void SetMusicVolume(float volume);
        void SetSFXVolume(float volume);
        void SetMasterVolume(float volume);
        float GetMusicVolume();
        float GetSFXVolume();
        float GetMasterVolume();

        SoundBuilder CreateSoundBuilder();
        void Play(SoundData soundData);

        void PlayMusic(AudioClip clip, bool loop = true, float volumeScale = 1f);
        void StopMusic();
        void Play(DefaultSounds defaultSound);
    }
}