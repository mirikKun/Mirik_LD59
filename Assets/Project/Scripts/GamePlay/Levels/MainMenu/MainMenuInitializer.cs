using Project.Scripts.GamePlay.Windows;
using Project.Scripts.Infrastructure.Sounds;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GamePlay.Levels.MainMenu
{
    public class MainMenuInitializer:MonoBehaviour
    {
        [SerializeField] private AudioClip _musicSound;
        private IWindowService _windowService;
        private ISoundsSystem _soundsSystem;

        [Inject]
        private void Construct(IWindowService windowService,ISoundsSystem soundsSystem)
        {
            _soundsSystem = soundsSystem;
            _windowService = windowService;
        }

        private void Start()
        {
            _windowService.Open(WindowId.MainMenu);
            _soundsSystem.PlayMusic(_musicSound);
        }
    }
}