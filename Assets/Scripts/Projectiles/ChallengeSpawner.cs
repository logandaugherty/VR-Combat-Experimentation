using UnityEngine;

public class ChallengeSpawner : MonoBehaviour
{
    private ChallengeScriptableObject m_challengeScriptableObject;
    private CountdownTimer countdowntimer;

    public void SetupSpawner(ChallengeScriptableObject challengeScriptableObject)
    {
        m_challengeScriptableObject = challengeScriptableObject;
    }
    private void Start()
    {
        countdowntimer = GetComponent<CountdownTimer>();
        countdowntimer.StartTimer(m_challengeScriptableObject.fireRate, () => { 
            Instantiate(m_challengeScriptableObject, transform); 
        }, true
        );
    }
}
