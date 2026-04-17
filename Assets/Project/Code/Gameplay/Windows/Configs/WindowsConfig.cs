using System.Collections.Generic;
using UnityEngine;

namespace Code.Gameplay.Windows.Configs
{
    [CreateAssetMenu(fileName = "WindowConfig", menuName = "Configs/Windows/Window Config")]
    public class WindowsConfig : ScriptableObject
    {
        public List<WindowConfig> WindowConfigs;
    }
}