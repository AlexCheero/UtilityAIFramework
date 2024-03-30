using UnityEngine;

namespace UtilityAI
{
    public abstract class BaseConsiderationAxis : ScriptableObject
    {
        public static T SpecifyContext<T>(IAIContext context) where T : IAIContext
        {
#if DEBUG
            if (context is not T)
            {
                Debug.LogError("wrong context type");
                return default;
            }
#endif
            return (T)context;
        }

        public abstract float GetNormalizedInput(IAIContext context);
    }
}