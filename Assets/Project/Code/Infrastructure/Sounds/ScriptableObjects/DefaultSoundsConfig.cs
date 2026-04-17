using System;
using System.Collections.Generic;
using Code.Infrastructure.Sounds.Enum;
using UnityEngine;

namespace Code.Infrastructure.Sounds.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DefaultSoundsConfig", menuName = "Configs/Sound/Default Sounds Config")]
    public class DefaultSoundsConfig : ScriptableObject
    {
        public List<DefaultSound> Sounds;

        public bool TryGet(DefaultSounds soundId, out SoundData soundData)
        {
            if (Sounds != null)
            {
                for (int i = 0; i < Sounds.Count; i++)
                {
                    DefaultSound entry = Sounds[i];
                    if (entry.SoundId == soundId)
                    {
                        soundData = entry.Data;
                        return true;
                    }
                }
            }

            soundData = null;
            return false;
        }
    }

    [Serializable]
    public class DefaultSound
    {
        public DefaultSounds SoundId;
        public SoundData Data;
    }
}

