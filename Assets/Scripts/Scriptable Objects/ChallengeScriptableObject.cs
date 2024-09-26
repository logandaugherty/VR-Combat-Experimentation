using UnityEngine;

public class ChallengeScriptableObject : ScriptableObject
{
    public float fireRate { get; private set; } = 1;
    public MeshRenderer model { get; private set; }

}