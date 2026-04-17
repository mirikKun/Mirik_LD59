using System.Collections.Generic;
using Code.Infrastructure.Sounds.ScriptableObjects;
using UnityEngine;
using UnityEngine.Pool;

namespace Code.Infrastructure.Sounds
{
    public class SoundManager : MonoBehaviour
    {
        private ObjectPool<SoundEmitter> _soundEmitterPool;
        private readonly List<SoundEmitter> _activeSoundEmitters = new();

        public readonly LinkedList<SoundEmitter> FrequentSoundEmitters = new();

        private bool _collectionCheck = true;
        private int _defaultCapacity = 10;
        private int _maxPoolSize = 64;
        private int _maxSoundInstances = 30;

        public void Construct(SoundMixersSO config)
        {
            _defaultCapacity = Mathf.Max(1, config.SfxPoolDefaultCapacity);
            _maxPoolSize = Mathf.Max(1, config.SfxPoolMaxSize);
            _maxSoundInstances = Mathf.Max(1, config.MaxFrequentSfxInstances);
            InitializePool();
        }

        public SoundBuilder CreateSoundBuilder() => new SoundBuilder(this);

        public bool CanPlaySound(SoundData data)
        {
            if (data == null || !data.FrequentSound)
                return true;

            if (FrequentSoundEmitters.Count >= _maxSoundInstances)
            {
                try
                {
                    FrequentSoundEmitters.First?.Value?.Stop();
                    return true;
                }
                catch
                {
                    Debug.Log("SoundEmitter is already released");
                }

                return false;
            }

            return true;
        }

        public SoundEmitter Get() => _soundEmitterPool.Get();

        public void ReturnToPool(SoundEmitter soundEmitter) => _soundEmitterPool.Release(soundEmitter);

        public void StopAll()
        {
            LinkedList<SoundEmitter> tempList = new LinkedList<SoundEmitter>(_activeSoundEmitters);

            foreach (SoundEmitter soundEmitter in tempList)
                soundEmitter.Stop();

            FrequentSoundEmitters.Clear();
        }

        private void InitializePool()
        {
            _soundEmitterPool = new ObjectPool<SoundEmitter>(
                CreateSoundEmitter,
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                _collectionCheck,
                _defaultCapacity,
                _maxPoolSize);
        }

        private SoundEmitter CreateSoundEmitter()
        {
            var go = new GameObject("[SoundEmitter]");
            go.transform.SetParent(transform, false);
            go.SetActive(false);

            go.AddComponent<AudioSource>();
            SoundEmitter soundEmitter = go.AddComponent<SoundEmitter>();
            soundEmitter.Construct(this);
            return soundEmitter;
        }

        private void OnTakeFromPool(SoundEmitter soundEmitter)
        {
            soundEmitter.gameObject.SetActive(true);
            _activeSoundEmitters.Add(soundEmitter);
        }

        private void OnReturnedToPool(SoundEmitter soundEmitter)
        {
            if (soundEmitter.Node != null)
            {
                FrequentSoundEmitters.Remove(soundEmitter.Node);
                soundEmitter.Node = null;
            }

            soundEmitter.gameObject.SetActive(false);
            _activeSoundEmitters.Remove(soundEmitter);
        }

        private void OnDestroyPoolObject(SoundEmitter soundEmitter)
        {
            Destroy(soundEmitter.gameObject);
        }
    }
}

