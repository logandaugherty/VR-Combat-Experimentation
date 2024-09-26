using UnityEngine;

[CreateAssetMenu(fileName = "Action", menuName = "Combat/DesignScriptableObject", order = 1)]
public class DesignScriptableObject : ScriptableObject
{
    [SerializeField] private string m_name;
    [SerializeField] private MeshRenderer m_meshRenderer;
    [SerializeField] private MeshRenderer m_defaultMeshRenderer;

    public void Init()
    {
        m_name = "Unknown";
        if (m_meshRenderer != null )
            m_meshRenderer = m_defaultMeshRenderer;
    }

    public void Init(string name, MeshRenderer meshRenderer)
    {
        m_name = name;
        m_meshRenderer = meshRenderer;
    }

    public void SetName(string name)
    {
        m_name = name;
    }
    public void SetModel(MeshRenderer meshRenderer)
    {
        m_meshRenderer = meshRenderer;
    }

    public string GetName()
    {
        return m_name;
    }
    public MeshRenderer GetMeshRenderer()
    {
        return m_meshRenderer;
    }

    public void Print()
    {
        Debug.Log($"Design Name: {m_name}\nModel: {m_meshRenderer}");
    }
}
