using Cysharp.Threading.Tasks;

namespace Code.Animations.EffectAnimations
{
    public interface IAnimation
    {
        UniTask PlayAnimation();
        float GetAnimationDuration();
        void SetStartState();
    }
}