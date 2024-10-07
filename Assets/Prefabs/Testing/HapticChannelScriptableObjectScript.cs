using UnityEngine;

[CreateAssetMenu(fileName = "HapticChannelScriptableObjectScript", menuName = "Scriptable Objects/HapticChannelScriptableObjectScript")]
public class HapticChannelScriptableObjectScript : ScriptableObject
{
    public delegate void EmissionAction(Vector3 source, float amp, float freq, float min, float max);
    private EmissionAction m_emissionAction;

    public void AddListener(EmissionAction emissionAction)
    {
        m_emissionAction += emissionAction;
    }

    public void RemoveListener(EmissionAction emissionAction)
    {
        m_emissionAction -= emissionAction;
    }

    public void EmitHaptic(Vector3 source, float amp, float freq, float min, float max)
    {
        m_emissionAction(source, amp, freq, min, max);
    }
}
