using UnityEngine;
using UnityEngine.UI;

public class UIProjectilesSpawn : MonoBehaviour
{
    [SerializeField] private GameObject m_projectile;
    private Button m_button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(() =>
        {
            Instantiate(m_projectile);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
