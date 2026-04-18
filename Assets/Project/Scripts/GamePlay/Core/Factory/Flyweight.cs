using System;
using UnityEngine;

namespace Project.Scripts.GamePlay.Core.Factory
{
    public abstract class Flyweight<TType>:MonoBehaviour where TType:Enum
    {
        public abstract TType Type { get; }
    }
}