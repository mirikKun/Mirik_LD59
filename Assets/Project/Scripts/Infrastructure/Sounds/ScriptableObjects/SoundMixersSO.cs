using UnityEngine;
using UnityEngine.Audio;

namespace Project.Scripts.Infrastructure.Sounds.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SoundMixersConfig", menuName = "Configs/Sound/SoundMixersConfig")]

    public class SoundMixersSO:ScriptableObject
    {
        [field:SerializeField] public AudioMixerGroup MusicGroup { get; private set; }
        [field:SerializeField] public AudioMixerGroup SoundGroup { get; private set; }
        [field: SerializeField] public float MinimalVolume { get; private set; } = -80;
        [field: SerializeField] public float DefaultVolume { get; private set; } = 0;

        [SerializeField] private string _masterVolumeParameter = "MasterVolume";
        [SerializeField] private string _musicVolumeParameter = "MusicVolume";
        [SerializeField] private string _sfxVolumeParameter = "SoundsVolume";

        public string MasterVolumeParameter => _masterVolumeParameter;
        public string MusicVolumeParameter => _musicVolumeParameter;
        public string SfxVolumeParameter => _sfxVolumeParameter;

        [field: Header("SFX Pool")]
        [field: SerializeField] public int SfxPoolDefaultCapacity { get; private set; } = 10;
        [field: SerializeField] public int SfxPoolMaxSize { get; private set; } = 64;
        [field: SerializeField] public int MaxFrequentSfxInstances { get; private set; } = 30;

        [field: Header("3D SFX")]
        [field: SerializeField] public float Sfx3DMinDistance { get; private set; } = 1f;
        [field: SerializeField] public float Sfx3DMaxDistance { get; private set; } = 50f;
    }
}