using Project.Scripts.GamePlay.Core.Entity;
using Project.Scripts.GamePlay.Player.PlayerResources;
using UnityEngine;

namespace Project.Scripts.GamePlay.Level.LevelObjects.LightHouse
{
    public class BrazierInteractable : BaseInteractable
    {
        [SerializeField] private LightHouseLightController _lightHouseLightController;
        [SerializeField] private float _lightHouseStrenghtPerWood = 4f;

        [SerializeField] private ParticleSystem _onInteractParticles;
        public override void Interact(BaseEntity entity)
        {


            if (entity is not ActorEntity actor ||
                !actor.TryGet(out WoodResourceController wood) )
            {
                base.Interact(entity);
                return;
            }

            if (!_lightHouseLightController.CanAddStrength())
                return;

            if (!wood.TryRemoveOne())
                return;

            _lightHouseLightController.AddStrength(_lightHouseStrenghtPerWood);
            _onInteractParticles.Play();
            base.Interact(entity);
        }
    }
}
