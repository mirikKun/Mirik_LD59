using Project.Scripts.GamePlay.Player.Abilities.General;

namespace Project.Scripts.GamePlay.Player.Abilities.Factory
{
    public interface IAbilitiesFactory
    {
        T CreateAbility<T>() where T:IAbility;
    }
}