using Cysharp.Threading.Tasks;

namespace Project.Scripts.Animations.EffectAnimations
{
    public interface IAnimation
    {
        UniTask PlayAnimation();
        float GetAnimationDuration();
        void SetStartState();
    }
}