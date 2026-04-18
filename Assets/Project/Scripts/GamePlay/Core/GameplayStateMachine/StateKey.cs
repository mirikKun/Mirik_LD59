using System;

namespace Project.Scripts.GamePlay.Core.GameplayStateMachine
{
    public struct StateKey
    {
        public Type Type { get; }
        public int Index { get; }

        public StateKey(Type type, int index = 0)
        {
            Type = type;
            Index = index;
        }

        public bool Equals(StateKey other)
        {
            return Type == other.Type && Index == other.Index;
        }

        public override bool Equals(object obj)
        {
            return obj is StateKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Index);
        }
    }
}