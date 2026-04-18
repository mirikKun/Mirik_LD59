using Project.Scripts.GamePlay.Core.Entity;
using Zenject;

namespace Project.Scripts.Infrastructure.Sounds.Behaviours
{
    public class SoundSource : EntityComponent
    {
        private ISoundsSystem _soundsSystem;

        [Inject]
        private void Construct(ISoundsSystem soundsSystem)
        {
            _soundsSystem = soundsSystem;
        }

        public void PlaySound(SoundData soundData)
        {
            _soundsSystem.Play(soundData);
        }
    }
}