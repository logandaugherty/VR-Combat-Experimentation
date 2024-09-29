using TMPro;
using UnityEngine;

public class UIProjectilesCompleted : MonoBehaviour
{
    [SerializeField] private EventActionScriptableObject m_actionEvent;
    private TextMeshProUGUI m_textMeshProUGUI;

    private int m_counter = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_textMeshProUGUI = GetComponent<TextMeshProUGUI>();

        m_actionEvent.AddActionListener(() => {
            m_counter++;
            m_textMeshProUGUI.text = m_counter.ToString();
        });
    }
}
