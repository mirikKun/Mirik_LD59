using Project.Scripts.GamePlay.Core.Entity;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.Indication
{
    public class AbilitiesIndicationController : EntityComponent
    {
        private RangeIndication _rangeIndication;
        public RangeIndication RangeIndication => _rangeIndication;

        public override void InitEntity(ActorEntity entity)
        {
            base.InitEntity(entity);
            _rangeIndication = new RangeIndication();
        }
    }
}