using Project.Scripts.Utils.ActionList.Editor;
using UnityEditor;

namespace Project.Scripts.GamePlay.Core.Entity.Effects.Editor
{
    [CustomPropertyDrawer(typeof(Effect))]
    public class EffectDrawer: ActionListAttributeDrawer<Effect>
    {
        
    }
}