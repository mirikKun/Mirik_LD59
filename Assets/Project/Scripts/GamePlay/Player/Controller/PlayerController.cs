using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Core.GameBehaviour.Services;
using Project.Scripts.GamePlay.Core.Input;

using Project.Scripts.GamePlay.Player.Abilities.Behaviours;
using Project.Scripts.GamePlay.Player.Abilities.Factory;
using Project.Scripts.GamePlay.Player.Abilities.Systems;
using Project.Scripts.GamePlay.Player.Health;
using Project.Scripts.GamePlay.Player.PlayerResources;
using Project.Scripts.GamePlay.Player.PlayerStateMachine;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GamePlay.Player.Controller
{
    [RequireComponent(typeof(PlayerMover))]
    public class PlayerController : EntityComponent, IGameUpdateable, IGameFixedUpdateable, IGameLateUpdateable
    {
        [SerializeField] private Transform _cameraViewTransform;
        [SerializeField] private Transform _targetTransform;


        private Transform _transform;
        private IInputReader _input;
        private IAbilitiesSystem _abilitiesSystem;
        
        private AbilitiesCaster _abilitiesCaster;
        private IUpdateService _updateService;
        private IAbilitiesFactory _abilitiesFactory;
        private Vector3 _startPosition;
        private CameraController CameraController => Entity.Get<CameraController>();


        public Transform Tr => _transform;
        public Transform CameraTrX => CameraController.CameraTrX;
        public Transform CameraTrY => CameraController.CameraTrY;
        public Transform TargetTr => _targetTransform;
        public Transform CameraViewTr => _cameraViewTransform;
        public IInputReader Input => _input;
     

        [Inject]
        private void Construct(IInputReader inputReader, IUpdateService updateService, IAbilitiesSystem abilitiesSystem,IAbilitiesFactory abilitiesFactory)
        {
            _abilitiesFactory = abilitiesFactory;
            _abilitiesSystem = abilitiesSystem;
            _updateService = updateService;
            _input = inputReader;
        }

        public override void InitEntity(ActorEntity entity)
        {
            base.InitEntity(entity);
            _transform = transform;
        }

        public override void StartEntity()
        {
            _startPosition = Entity.Get<PlayerMover>().Tr.position;
            _input.EnablePlayerActions();
            Entity.Get<AbilitiesCaster>().Init();

            _updateService.PlayerUpdate.Register(this);
            _updateService.PlayerFixedUpdate.Register(this);
            _updateService.LateUpdate.Register(this);
            Entity.Get<PlayerHealth>().Died+=RespawnPlayer;
        }

        public void SetRespawnPosition(Vector3 position)
        {
            _startPosition = position;
        }
        private void RespawnPlayer(BaseEntity baseEntity)
        {
            ResetPlayerPosition();
            Entity.Get<PlayerHealth>().Reset();
        }

        public void ResetPlayerPosition()
        {
            Entity.Get<PlayerMover>().Tr.position = _startPosition;
        }

        private void OnDestroy()
        {
            _updateService.PlayerUpdate.Unregister(this);
            _updateService.PlayerFixedUpdate.Unregister(this);
            _updateService.LateUpdate.Unregister(this);
        }
        
        public void GameUpdate(float deltaTime)
        {
            Entity.Get<PlayerMover>().Tick(deltaTime);

            Entity.Get<PlayerStateMachineContainer>().Tick(deltaTime);
            Entity.Get<PlayerInteractor>().Tick();
            Entity.Get<PlayerResourcesController>().Tick(deltaTime);
            Entity.Get<PlayerEffects.PlayerEffects>().Tick(deltaTime);
            
            Entity.Get<AbilitiesCaster>().Tick(deltaTime);

        }

        public void GameLateUpdate(float deltaTime)
        {
            CameraController.TickLateUpdate(deltaTime);
            Entity.Get<PlayerMover>().LateTick(deltaTime);
        }

        public void GameFixedUpdate(float fixedDeltaTime)
        {
            Entity.Get<PlayerMover>().SetVelocity(Vector3.zero);
            Entity.Get<PlayerMover>().CheckForGround(fixedDeltaTime);
            Entity.Get<PlayerStateMachineContainer>().FixedTick(fixedDeltaTime);

            Entity.Get<CeilingDetector>().Reset();
            Entity.Get<WallDetector>().Reset();
            Entity.Get<PlayerMover>().FixedTick(fixedDeltaTime);
        }

  
        public Vector3 GetInputMovementDirection()
        {
            Vector3 direction = CameraTrX == null
                ? _transform.right * _input.Direction.x + _transform.forward * _input.Direction.y
                : Vector3.ProjectOnPlane(CameraTrX.right, _transform.up).normalized * _input.Direction.x +
                  Vector3.ProjectOnPlane(CameraTrX.forward, _transform.up).normalized * _input.Direction.y;

            return direction.magnitude > 1f ? direction.normalized : direction;
        }

    }
}