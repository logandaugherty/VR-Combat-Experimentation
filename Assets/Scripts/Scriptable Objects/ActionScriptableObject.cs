using UnityEngine;

[CreateAssetMenu(fileName = "Action", menuName = "Combat/ActionScriptableObject", order = 1)]
public class ActionScriptableObject : ScriptableObject
{
	[SerializeField] private int m_dmg;
    [SerializeField] private string m_name;
    
    public void Init()
    {
        m_dmg = 0;
        m_name = "Unknown";
    }

    public void Init(string name, int dmg)
    {
        m_dmg = dmg;
        m_name = name;
    }

    public void SetDmg(int dmg)
    {
        m_dmg = dmg;
    }

    public void SetName(string name)
    {
        m_name = name;
    }

    public int GetDmg()
    {
        return m_dmg;
    }

    public string GetName()
    {
        return m_name;
    }

    public void Print()
    {
        Debug.Log($"Action Name: {m_name}\nDmg: {m_dmg}");
    }
}
