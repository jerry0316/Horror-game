using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthText.text = $"Health: {currentHealth}/{maxHealth}";
    }
}
