using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.Infrastructure.StaticData;
using UnityEngine;
using Zenject;

namespace Project.Scripts.GamePlay.Player.PlayerEffects
{
    public class PlayerEffects : EntityComponent
    {
        [SerializeField] private HookEffects _hookEffects;
        [SerializeField] private TrajectoryEffects _trajectoryEffects;
        [SerializeField] private CameraMovingEffects _cameraMovingEffects;
        [SerializeField] private TimeSlowEffect _timeSlowEffect;
       // [SerializeField] private CastingEffects _castingEffects;


        public HookEffects HookEffects => _hookEffects;
        public TrajectoryEffects TrajectoryEffects => _trajectoryEffects;
        public CameraMovingEffects CameraMovingEffects => _cameraMovingEffects;
      //  public CastingEffects CastingEffects => _castingEffects;
        public TimeSlowEffect TimeSlowEffect => _timeSlowEffect;

        private IStaticDataService _staticDataService;

        [Inject]
        private void Construct(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public override void StartEntity()
        {
            // _hookEffects.InitEffect(Entity);
            // _trajectoryEffects.InitEffect(Entity);
            _cameraMovingEffects.InitEffect(Entity);
            // _timeSlowEffect.InitEffect(Entity);
        }

        public void Tick(float deltaTime)
        {
            CameraMovingEffects.Tick(deltaTime);
           // CastingEffects.Tick(deltaTime);
        }
    }
}