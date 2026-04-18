using System;
using System.Collections.Generic;
using Project.Scripts.Infrastructure.Sounds.Enum;
using UnityEngine;

namespace Project.Scripts.Infrastructure.Sounds.ScriptableObjects
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

