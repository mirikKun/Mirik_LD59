using System;

namespace Project.Scripts.GamePlay.Core.GameplayStateMachine
{
  public class TransitionConfiguration
    {
        public Type FromState;
        public int FromIndex = 0;
        public Type ToState;
        public int ToIndex = 0;
        public Func<bool> Condition;

        public TransitionConfiguration(Type fromState, Type toState, Func<bool> condition)
        {
            FromState = fromState;
            ToState = toState;
            Condition = condition;
        }

        public TransitionConfiguration(Type fromState, int fromIndex, Type toState, int toIndex, Func<bool> condition)
        {
            FromState = fromState;
            FromIndex = fromIndex;
            ToState = toState;
            ToIndex = toIndex;
            Condition = condition;
        }

        public static TransitionConfiguration GetConfiguration<TFrom,TTo>(Func<bool> condition) where TFrom:IState where TTo:IState
        {
            return new TransitionConfiguration(typeof(TFrom), typeof(TTo), condition);
        }

        public static TransitionConfiguration GetConfiguration<TFrom,TTo>(Func<bool> condition, int fromIndex, int toIndex) where TFrom:IState where TTo:IState
        {
            return new TransitionConfiguration(typeof(TFrom), fromIndex, typeof(TTo), toIndex, condition);
        }

        public static TransitionConfiguration GetConfiguration<TFrom>(IState toState, Func<bool> condition, Func<bool> extraCondition) where TFrom:IState 
        {
            return new TransitionConfiguration(typeof(TFrom), toState.GetType(), 
                () => condition() && extraCondition());
        }

        public static TransitionConfiguration GetConfiguration<TFrom>(IState toState, int toIndex, Func<bool> condition, Func<bool> extraCondition) where TFrom:IState 
        {
            return new TransitionConfiguration(typeof(TFrom), 0, toState.GetType(), toIndex, 
                () => condition() && extraCondition());
        }

        public static TransitionConfiguration GetConfiguration<TTo>(IState fromState, Func<bool> condition) where TTo:IState 
        {
            return new TransitionConfiguration(fromState.GetType(), typeof(TTo), condition);
        }

        public static TransitionConfiguration GetConfiguration<TTo>(IState fromState, int fromIndex, Func<bool> condition) where TTo:IState 
        {
            return new TransitionConfiguration(fromState.GetType(), fromIndex, typeof(TTo), 0, condition);
        }

        public static TransitionConfiguration GetConfiguration<TFrom,TTo>(Func<bool> condition, float possibility) where TFrom:IState where TTo:IState
        {
            return new TransitionConfiguration(typeof(TFrom), typeof(TTo), 
                () => condition() && UnityEngine.Random.value >= possibility);
        }

        public static TransitionConfiguration GetConfiguration<TFrom,TTo>(Func<bool> condition, float possibility, int fromIndex, int toIndex) where TFrom:IState where TTo:IState
        {
            return new TransitionConfiguration(typeof(TFrom), fromIndex, typeof(TTo), toIndex, 
                () => condition() && UnityEngine.Random.value >= possibility);
        }
    }     
}