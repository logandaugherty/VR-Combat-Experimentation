using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "Events/EventFloatScriptableObject", order = 1)]
public class EventFloatScriptableObject : ScriptableObject
{
    Action<float> m_action;

    public void AddActionListener(Action<float> action)
    {
        m_action += action;
    }

    public void RemoveActionListener(Action<float> action)
    {
        m_action -= action;
    }

    public void TriggerAction(float value)
    {
        m_action.Invoke(value);
    }   
}
