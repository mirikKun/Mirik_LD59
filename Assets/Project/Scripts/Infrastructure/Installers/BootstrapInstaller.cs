using Project.Scripts.GamePlay.Cameras.Provider;
using Project.Scripts.GamePlay.Common.Random;
using Project.Scripts.GamePlay.Common.Time;
using Project.Scripts.GamePlay.Core.GameBehaviour.Services;
using Project.Scripts.GamePlay.Core.Input;
using Project.Scripts.GamePlay.Input.Service;
using Project.Scripts.GamePlay.Level.Factories;
using Project.Scripts.GamePlay.Levels;
using Project.Scripts.GamePlay.Player.Abilities.Factory;
using Project.Scripts.GamePlay.Player.Abilities.Systems;
using Project.Scripts.GamePlay.Player.Inventory.Systems;
using Project.Scripts.GamePlay.Windows;
using Project.Scripts.Infrastructure.AssetManagement;
using Project.Scripts.Infrastructure.Loading;
using Project.Scripts.Infrastructure.Progress.Provider;
using Project.Scripts.Infrastructure.Saving;
using Project.Scripts.Infrastructure.Settings;
using Project.Scripts.Infrastructure.Sounds;
using Project.Scripts.Infrastructure.States.Factory;
using Project.Scripts.Infrastructure.States.GameStates;
using Project.Scripts.Infrastructure.States.StateMachine;
using Project.Scripts.Infrastructure.StaticData;
using Zenject;

namespace Project.Scripts.Infrastructure.Installers
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
            BindPlayerAbilities();
        }

        private void BindGameStateMachine()
        {
            Container.Bind<IStateFactory>().To<StateFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();
        }
        private void BindGameStates()
        {
            Container.BindInterfacesAndSelfTo<BootstrapState>().AsSingle();
            Container.BindInterfacesAndSelfTo<InitializeProgressState>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadingHomeScreenState>().AsSingle();
            Container.BindInterfacesAndSelfTo<HomeScreenState>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadingGameplayState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayEnterState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameLoopState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameWonState>().AsSingle();
            


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
            Container.Bind<IInteractablesFactory>().To<InteractablesFactory>().AsSingle();
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

        private void BindPlayerAbilities()
        {
            Container.Bind<IAbilitiesFactory>().To<AbilitiesFactory>().AsSingle();
            Container.Bind<IAbilitiesSystem>().To<AbilitiesSystem>().AsSingle();

            Container.Bind<IInventorySystem>().To<InventorySystem>().AsSingle();

        }

        private void BindCommonServices()
        {
            Container.Bind<IRandomService>().To<UnityRandomService>().AsSingle();
            Container.Bind<ITimeService>().To<UnityTimeService>().AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            Container.Bind<IUpdateService>().To<UpdateService>().AsSingle();

            Container.Bind<IWindowFactory>().To<WindowFactory>().AsSingle();
            Container.Bind<IWindowService>().To<WindowService>().AsSingle();
        }

        private void BindInputService()
        {
            Container.Bind<IInputService>().To<StandaloneInputService>().AsSingle();
            Container.Bind<IInputReader>().To<InputReader>().AsSingle();

        }

        public void Initialize()
        {
            Container.Resolve<IGameStateMachine>().Enter<BootstrapState>();
        }
    }
}