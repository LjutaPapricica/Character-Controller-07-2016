using UnityEngine;
using System.Collections;

public class HealthComponent : MonoBehaviour
{
    private int m_currentHealth = 100;
    private int m_maximumHealth = 100;

    void Start()
    {
    }

    public void Heal(int health)
    {
        m_currentHealth = Mathf.Min(m_currentHealth + health, m_maximumHealth);
    }

    public void Damage(int health)
    {
        m_currentHealth = Mathf.Max(m_currentHealth - health, 0);

        if(m_currentHealth == 0)
        {
            Destroy(gameObject);
        }
    }
}
