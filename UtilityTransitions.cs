using System;
using System.Collections.Generic;

namespace UtilityAI
{
    public class UtilityTransitions<ActionType, PayloadType> where ActionType : struct, Enum
    {
        public delegate void TransitionCallback(PayloadType payload);

        private Dictionary<ActionType, TransitionCallback> _exitCallbacks;
        private Dictionary<ActionType, TransitionCallback> _enterCallbacks;
        private Dictionary<ActionType, Dictionary<ActionType, TransitionCallback>> _transitions;

        public void AddExitCallback(ActionType from, TransitionCallback callback)
        {
            _exitCallbacks ??= new();
            if (!_exitCallbacks.ContainsKey(from))
                _exitCallbacks[from] = callback;
            else
                _exitCallbacks[from] += callback;
        }

        public void AddEnterCallback(ActionType to, TransitionCallback callback)
        {
            _enterCallbacks ??= new();
            if (!_enterCallbacks.ContainsKey(to))
                _enterCallbacks[to] = callback;
            else
                _enterCallbacks[to] += callback;
        }

        public void AddTransition(ActionType from, ActionType to, TransitionCallback callback)
        {
            _transitions ??= new();
            if (!_transitions.ContainsKey(from))
                _transitions[from] = new();
            if (!_transitions[from].ContainsKey(from))
                _transitions[from][to] = callback;
            else
                _transitions[from][to] += callback;
        }

        public void OnActionChange(ActionType from, ActionType to, PayloadType payload)
        {
            if (_exitCallbacks != null && _exitCallbacks.ContainsKey(from))
                _exitCallbacks[from](payload);

            if (_transitions != null && _transitions.ContainsKey(from) && _transitions[from].ContainsKey(to))
                _transitions[from][to](payload);

            if (_enterCallbacks != null && _enterCallbacks.ContainsKey(to))
                _enterCallbacks[to](payload);
        }
    }
}
