using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.GamePlay.Windows.Configs
{
    [CreateAssetMenu(fileName = "WindowConfig", menuName = "Configs/Windows/Window Config")]
    public class WindowsConfig : ScriptableObject
    {
        public List<WindowConfig> WindowConfigs;
    }
}