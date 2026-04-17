using Code.Gameplay.Cameras.Provider;
using Code.Gameplay.Common.Random;
using Code.Gameplay.Common.Time;
using Code.Gameplay.Input.Service;
using Code.Gameplay.Levels;
using Code.Gameplay.Windows;
using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.Loading;
using Code.Infrastructure.Progress.Provider;
using Code.Infrastructure.Saving;
using Code.Infrastructure.Settings;
using Code.Infrastructure.Sounds;
using Code.Infrastructure.States.Factory;
using Code.Infrastructure.States.GameStates;
using Code.Infrastructure.States.StateMachine;
using Code.Infrastructure.StaticData;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller, ICoroutineRunner, IInitializable
    {
        public override void InstallBindings()
        {
            BindInputService();
            BindGameStateMachine();
            BindGameStates();
            BindInfrastructureServices();
            BindAssetManagementServices();
            BindCommonServices();
            BindSettings();
            BindGameplayServices();
            BindCameraProvider();
        }

        private void BindGameStateMachine()
        {
            Container.Bind<IStateFactory>().To<StateFactory>().AsSingle();
            Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();
        }
        private void BindGameStates()
        {
            Container.BindInterfacesAndSelfTo<BootstrapState>().AsSingle();
            Container.BindInterfacesAndSelfTo<InitializeProgressState>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadingHomeScreenState>().AsSingle();
            Container.BindInterfacesAndSelfTo<HomeScreenState>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadingGameplayState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayEnterState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameloopLoopState>().AsSingle();
            


        }

        private void BindCameraProvider()
        {
            Container.BindInterfacesAndSelfTo<CameraProvider>().AsSingle();
        }

        private void BindGameplayServices()
        {
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
            Container.Bind<ISoundsSystem>().To<SoundsSystem>().AsSingle();
            Container.Bind<ILevelDataProvider>().To<LevelDataProvider>().AsSingle();
            Container.Bind<IProgressProvider>().To<ProgressProvider>().AsSingle();
        }

        private void BindSettings()
        {
            Container.Bind<ISavingService>().To<PlayerPrefsSavingService>().AsSingle();
            Container.Bind<ISettingsService>().To<SettingsService>().AsSingle();
        }

        private void BindInfrastructureServices()
        {
            Container.BindInterfacesTo<BootstrapInstaller>().FromInstance(this).AsSingle();
        }

        private void BindAssetManagementServices()
        {
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
        }

        private void BindCommonServices()
        {
            Container.Bind<IRandomService>().To<UnityRandomService>().AsSingle();
            Container.Bind<ITimeService>().To<UnityTimeService>().AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            
            Container.Bind<IWindowFactory>().To<WindowFactory>().AsSingle();
            Container.Bind<IWindowService>().To<WindowService>().AsSingle();
        }

        private void BindInputService()
        {
            Container.Bind<IInputService>().To<StandaloneInputService>().AsSingle();
        }

        public void Initialize()
        {
            Container.Resolve<IGameStateMachine>().Enter<BootstrapState>();
        }
    }
}