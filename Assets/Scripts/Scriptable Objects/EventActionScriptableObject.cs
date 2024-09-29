using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "Events/EventActionScriptableObject", order = 1)]
public class EventActionScriptableObject : ScriptableObject
{
    Action m_action;

    public void AddActionListener(Action action)
    {
        m_action += action;
    }

    public void RemoveActionListener(Action action)
    {
        m_action -= action;
    }

    public void TriggerAction()
    {
        m_action.Invoke();
    }   
}
