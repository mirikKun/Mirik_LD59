using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Utils
{
    [Serializable]
    public struct FloatRange
    {
        [SerializeField]private float _min, _max;
        public float Min=>_min;
        public float Max=>_max;

        public float RandomValueInRange
        {
            get => Random.Range(_min, _max);
        }

        public FloatRange(float value)
        {
            _min = _max=value;
        }
        public FloatRange(float min, float max)
        {
            _min = min;
            _max = max;
        }
        public static implicit operator FloatRange(float value)
        {
            return new FloatRange(value);
        }
        // public static implicit operator float (FloatRange range)
        // {
        //     return range.RandomValueInRange;
        // }
    }

    public class FloatRangeSliderAttribute : PropertyAttribute
    {
        public float Min { get; private set; }
        public float Max { get; private set; }

        public FloatRangeSliderAttribute(float min, float max)
        {
            Min = min;
            Max = max < min ? min : max;
        }  
        public FloatRangeSliderAttribute(int min, int max)
        {
            Min = min;
            Max = max < min ? min : max;
        }
    }
}