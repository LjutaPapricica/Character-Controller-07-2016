using UnityEngine;
using System.Collections;

public class HealthComponent : MonoBehaviour
{
    public  int m_maximumHealth = 30;
    private int m_currentHealth;

    void Start()
    {
        m_currentHealth = m_maximumHealth;
    }

    public void Heal(int health)
    {
        m_currentHealth = Mathf.Min(m_currentHealth + health, m_maximumHealth);
    }

    public void Damage(int health, Vector3 position, Vector3 force)
    {
        m_currentHealth = Mathf.Max(m_currentHealth - health, 0);

        if(m_currentHealth == 0)
        {
            // Spawn a corpse prefab.
            GameObject corpse = (GameObject)Instantiate(Resources.Load("Corpse"));
            corpse.transform.position += gameObject.transform.position;
            corpse.transform.rotation = gameObject.transform.rotation;
            corpse.transform.parent = GameObject.Find("Entities").transform;
            corpse.name = "Corpse";

            Rigidbody rigidbody = corpse.GetComponent<Rigidbody>();
            rigidbody.AddForceAtPosition(force, position);

            // Destroy the object.
            DestroyImmediate(gameObject);
        }
    }
}
