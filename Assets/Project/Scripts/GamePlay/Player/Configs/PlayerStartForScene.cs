using Project.Scripts.GamePlay.Player.Abilities.Configs;
using Project.Scripts.GamePlay.Player.Inventory.Configs;

namespace Project.Scripts.GamePlay.Player.Configs
{
    public readonly struct PlayerStartForScene
    {
        public PlayerStartAbilities Abilities { get; }
        public PlayerStartInventory Inventory { get; }

        public PlayerStartForScene(PlayerStartAbilities abilities, PlayerStartInventory inventory)
        {
            Abilities = abilities;
            Inventory = inventory;
        }
    }
}
