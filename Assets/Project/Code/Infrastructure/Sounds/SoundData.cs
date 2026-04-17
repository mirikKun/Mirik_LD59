using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Code.Infrastructure.Sounds
{
    [Serializable]
    public class SoundData
    {
        public AudioClip Clip;
        public AudioMixerGroup MixerGroup;
        public bool Loop;
        public bool PlayOnAwake;
        public bool FrequentSound;

        public bool Mute;
        public bool BypassEffects;
        public bool BypassListenerEffects;
        public bool BypassReverbZones;

        public int Priority = 128;
        public float Volume = 1f;
        public float Pitch = 1f;
        public float PanStereo;
        public float SpatialBlend;
        public float ReverbZoneMix = 1f;
        public float DopplerLevel = 1f;
        public float Spread;

        public float MinDistance = 1f;
        public float MaxDistance = 500f;

        public bool IgnoreListenerVolume;
        public bool IgnoreListenerPause;

        public AudioRolloffMode RolloffMode = AudioRolloffMode.Logarithmic;

        public SoundData()
        {
            Priority = 128;
            Volume = 1f;
            Pitch = 1f;
            ReverbZoneMix = 1f;
            DopplerLevel = 1f;

            MinDistance = 1f;
            MaxDistance = 500f;
            AudioRolloffMode RolloffMode = AudioRolloffMode.Logarithmic;
        }
    }
}