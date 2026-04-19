using Project.Scripts.GamePlay.Cameras.Provider;
using Project.Scripts.GamePlay.Level.Factories;
using Project.Scripts.GamePlay.Levels;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Infrastructure.Installers
{
    public class LevelInitializer : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _lighthouseTarget;
        [SerializeField] private Transform _interactablesParent;
        private ICameraProvider _cameraProvider;
        private ILevelDataProvider _levelDataProvider;
        private IInteractablesFactory _interactablesFactory;

        [Inject]
        private void Construct(ICameraProvider cameraProvider, ILevelDataProvider levelDataProvider,IInteractablesFactory  interactablesFactory)
        {
            _levelDataProvider = levelDataProvider;
            _cameraProvider = cameraProvider;
            _interactablesFactory = interactablesFactory;
        }

        public void Start()
        {
            _interactablesFactory.SetupInteractablesParent(_interactablesParent);
            _levelDataProvider.SetStartPoint(_startPoint);
                _levelDataProvider.SetLighthouseTarget(_lighthouseTarget);
            _cameraProvider.SetMainCamera(_mainCamera);
        }
    }
}