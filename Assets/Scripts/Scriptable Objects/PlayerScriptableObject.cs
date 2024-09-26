using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Action", menuName = "Combat/PlayerScriptableObject", order = 1)]
public class PlayerScriptableObject : ScriptableObject
{
	[SerializeField] string m_name = "Unkown";

    [SerializeField] private int health = 100;

    [SerializeField] DesignScriptableObject m_head;
    [SerializeField] DesignScriptableObject m_leftHand;
    [SerializeField] DesignScriptableObject m_rightHand;

    List<ActionScriptableObject> m_actions;

    public void Init()
    {
        m_name = "Unknown";
        m_head.Init();
        m_leftHand.Init();
        m_rightHand.Init();
    }

    public void Init(string name)
    {
        m_name = name;
        m_head.Init();
        m_leftHand.Init();
        m_rightHand.Init();
    }

    public void SetName(string name)
    {
        m_name = name;
    }

    public void SetDesign(DesignScriptableObject head, DesignScriptableObject leftHand, DesignScriptableObject rightHand)
    {
        m_head = head;
        m_leftHand = leftHand;
        m_rightHand = rightHand;
    }

    public void SetHead(DesignScriptableObject head)
    {
        m_head = head;
    }

    public void SetLeftHand(DesignScriptableObject leftHand)
    {
        m_leftHand = leftHand;
    }

    public void SetRightHand(DesignScriptableObject rightHand)
    {
        m_rightHand = rightHand;
    }

    public string GetName()
    {
        return m_name;
    }

    public DesignScriptableObject GetHead()
    {
        return m_head;
    }

    public DesignScriptableObject GetLeftHand()
    {
        return m_leftHand;
    }

    public DesignScriptableObject GetRightHand()
    {
        return m_rightHand;
    }

    public void Print()
    {
        Debug.Log($"Player Name: {m_name}\n" +
            $"Head:");
        m_head.Print();

        Debug.Log("Left Hand:");
        m_leftHand.Print();

        Debug.Log("Right Hand:");
        m_rightHand.Print();

        Debug.Log("Actions:");
        foreach (var action in m_actions)
        {
            action.Print();
        }
    }

    public void Damage(int interval)
    {
        health -= interval;
    }

    public void Heal(int interval)
    {
        health += interval;
    }
}
