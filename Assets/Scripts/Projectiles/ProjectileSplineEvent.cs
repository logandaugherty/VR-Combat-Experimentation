using UnityEngine;
using UnityEngine.Splines;

public class ProjectileSplineEvent : MonoBehaviour
{
    [SerializeField] private EventActionScriptableObject actionEvent;
    private SplineAnimate m_splineAnimate;
    private bool m_isComplete;

    private void Start()
    {
        m_splineAnimate = GetComponent<SplineAnimate>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_splineAnimate.IsPlaying && !m_isComplete)
        {
            m_isComplete = true;
            actionEvent.TriggerAction();
        }
    }
}
