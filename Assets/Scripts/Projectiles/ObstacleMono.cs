using UnityEngine;
using UnityEngine.Splines;

public class ObstacleMono : MonoBehaviour
{
    [SerializeField] private Vector3 m_minRange;
    [SerializeField] private Vector3 m_maxRange;

    [SerializeField] private SplineContainer m_splineContainer;
    [SerializeField] private SplineAnimate m_splineAnimate;

    [SerializeField] private PlayerScriptableObject m_playerScriptableObject;


    private void Start()
    {
        Vector3 midPoint = new Vector3(
            Random.Range(m_minRange.x, m_maxRange.x), 
            Random.Range(m_minRange.y, m_maxRange.y), 
            Random.Range(m_minRange.z, m_maxRange.z));
        m_splineContainer.Spline.SetKnot(1, new BezierKnot(midPoint) );
    }

    private void Update()
    {
        if (!m_splineAnimate.IsPlaying)
        {
            m_playerScriptableObject.Damage(10);
            Destroy(gameObject);
        }
    }
}
