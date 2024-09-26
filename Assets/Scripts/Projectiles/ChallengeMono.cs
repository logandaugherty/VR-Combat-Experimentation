using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ChallengeMono : MonoBehaviour
{
    [SerializeField] private List<ChallengeScriptableObject> challenges;
    [SerializeField] private GameObject challengeSpawnerPrefab;
    [SerializeField] private float rangeDeg;

    private void Start()
    {
        for (int i=0; i < challenges.Count; i++)
        {
            var newSpawner = Instantiate(challengeSpawnerPrefab);
            
            Vector3 rotationEuler = Vector3.up;
            rotationEuler *= (i - challenges.Count / 2) * 45;
            newSpawner.transform.Rotate(rotationEuler);

            ChallengeScriptableObject ch = challenges[i];
            newSpawner.GetComponent<ChallengeSpawner>().SetupSpawner(ch);
        }
    }
}
