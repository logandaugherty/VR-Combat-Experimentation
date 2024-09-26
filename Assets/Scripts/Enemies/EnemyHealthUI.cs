using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;
    [SerializeField] private EventScriptableObject eventScriptableObject;
    [SerializeField] private Slider slider;

    public void Damage(float amount)
    {
        health -= amount;
        UpdateSlider();
    }
    public void Heal(float amount)
    {
        health += amount;
        UpdateSlider();
    }

    private void UpdateSlider()
    {
        slider.value = health / maxHealth;
    }

    private void Start()
    {
        health = maxHealth;

        slider = GetComponent<Slider>();
        UpdateSlider();

        eventScriptableObject.AddActionListener(
            (float value) =>
            {
                Damage(value);
                UpdateSlider();
            });
    }
}
