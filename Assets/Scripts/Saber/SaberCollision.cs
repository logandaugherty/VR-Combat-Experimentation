using UnityEngine;

public class SaberCollision : MonoBehaviour
{
    private HapticPlayer m_hapticPlayer;
    private bool m_isPlaying;

    private void Start()
    {
        m_hapticPlayer = GetComponent<HapticPlayer>();
        Invoke(nameof(EnableCollision), 0.5f);
    }

    private void EnableCollision()
    {
        m_isPlaying = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!m_isPlaying)
            return;
        if (collider.CompareTag("Saber"))
        {
            m_hapticPlayer.Play();
            Debug.Log($"Turning on Haptics for {transform.name}");
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (!m_isPlaying)
            return;
        if (collider.CompareTag("Saber"))
        {
            m_hapticPlayer.Pause();
            Debug.Log($"Turning off Haptics for {transform.name}");
        }
    }
}
