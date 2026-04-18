using Project.Scripts.GamePlay.Cameras.Provider;
using Project.Scripts.GamePlay.Levels;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Infrastructure.Installers
{
    public class LevelInitializer : MonoBehaviour, IInitializable
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Transform _startPoint;
        private ICameraProvider _cameraProvider;
        private ILevelDataProvider _levelDataProvider;

        [Inject]
        private void Construct(ICameraProvider cameraProvider, ILevelDataProvider levelDataProvider)
        {
            _levelDataProvider = levelDataProvider;
            _cameraProvider = cameraProvider;
        }

        public void Initialize()
        {
            _levelDataProvider.SetStartPoint(_startPoint);
            _cameraProvider.SetMainCamera(_mainCamera);
        }
    }
}