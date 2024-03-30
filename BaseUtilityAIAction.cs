using System;
using System.Collections.Generic;
using UnityEngine;

namespace UtilityAI
{
    public abstract class BaseUtilityAIAction<T> : ScriptableObject where T : struct, Enum
    {
        public abstract T ActionType { get; }

        [Serializable]
        private struct AxisCurvePair
        {
            public BaseConsiderationAxis Axis;
            public AnimationCurve Curve;
        }

        [SerializeField]
        private List<AxisCurvePair> Considerations;

        [SerializeField]
        protected float _mult = 1.0f;
        protected float Mult => _mult;

        public float GetUtility(IAIContext context)
        {
            float result = 1;
            foreach (var axis in Considerations)
            {
                var normalizedInput = axis.Axis.GetNormalizedInput(context);
                var mult = axis.Curve.Evaluate(normalizedInput);
                result *= mult;
                if (result == 0)
                    break;
            }
            return result * Mult;
        }
    }
}