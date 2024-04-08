using System;
using UnityEngine;

namespace UtilityAI
{
    public struct SwitchEquivalentActionSettings
    {
        public float lastActionChooseTime;
        public float actionChooseCD;
        public float actionChooseProbability;
    }

    public class UtilityAISolver
    {
        public T Solve<T>(
            UtilityAIAction<T>[] actions,
            IAIContext botContext,
            SwitchEquivalentActionSettings switchEquivalentSettings,
            T bestAction) where T : struct, Enum
        {
            var bestScore = 0.0f;
            for (int i = 0; i < actions.Length; i++)
            {
                var newScore = actions[i].GetUtility(botContext);
                var shouldChangeAction = bestScore < newScore ||
                    (newScore == bestScore &&
                    Time.time - switchEquivalentSettings.lastActionChooseTime > switchEquivalentSettings.actionChooseCD &&
                    UnityEngine.Random.value >= switchEquivalentSettings.actionChooseProbability);

                if (shouldChangeAction)
                {
                    bestScore = newScore;
                    bestAction = actions[i].ActionType;
                }
            }

            return bestAction;
        }
    }
}
