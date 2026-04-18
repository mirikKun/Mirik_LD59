using Project.Scripts.GamePlay.Player.Abilities.General;
using Zenject;

namespace Project.Scripts.GamePlay.Player.Abilities.Factory
{
    public class AbilitiesFactory:IAbilitiesFactory
    {
        private DiContainer _diContainer;

        public AbilitiesFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public T CreateAbility<T>() where T:IAbility
        {
            T ability = _diContainer.Instantiate<T>();

            return ability;
        }
    }
}