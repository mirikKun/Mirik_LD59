using System;
using Project.Scripts.GamePlay.Levels.Enum;
using Project.Scripts.GamePlay.Player.Abilities.Configs;
using Project.Scripts.GamePlay.Player.Inventory.Configs;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.Configs
{
    [CreateAssetMenu(fileName = "PlayerStartByScene", menuName = "Configs/Player/Player Start By Scene")]
    public class PlayerStartBySceneConfig : ScriptableObject
    {
        [SerializeField] private SceneEntry _defaultSceneSetup;
        [SerializeField] private SceneEntry _tutorialtSceneSetup;
        [SerializeField] private SceneEntry[] _perScene;

        [Serializable]
        public struct SceneEntry
        {
            public Scenes Scene;
            public PlayerStartAbilities Abilities;
            public PlayerStartInventory Inventory;
        }

        public PlayerStartForScene GetForScene(Scenes scene)
        {
            if (_perScene != null)
            {
                foreach (SceneEntry entry in _perScene)
                {
                    if (entry.Scene != scene)
                        continue;

                    PlayerStartAbilities abilities = entry.Abilities != null ? entry.Abilities : _defaultSceneSetup.Abilities;
                    PlayerStartInventory inventory = entry.Inventory != null ? entry.Inventory : _defaultSceneSetup.Inventory;
                    return new PlayerStartForScene(abilities, inventory);
                }
            }

            return new PlayerStartForScene(_defaultSceneSetup.Abilities, _defaultSceneSetup.Inventory);
        }

        public PlayerStartForScene GetFirstTutorialSetup() =>
            new PlayerStartForScene(_tutorialtSceneSetup.Abilities, _tutorialtSceneSetup.Inventory);
    }
}
