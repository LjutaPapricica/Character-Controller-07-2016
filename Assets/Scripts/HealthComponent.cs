using UnityEngine;
using System.Collections;

public class HealthComponent : MonoBehaviour
{
    public int m_currentHealth = 30;
    public int m_maximumHealth = 30;

    public void Heal(int health)
    {
        m_currentHealth = Mathf.Min(m_currentHealth + health, m_maximumHealth);
    }

    public void Damage(int health, Vector3 position, Vector3 force)
    {
        m_currentHealth = Mathf.Max(m_currentHealth - health, 0);

        if(m_currentHealth == 0)
        {
            // Disable collisition of the object.
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.detectCollisions = false;

            // Destroy the object.
            Destroy(gameObject);

            // Create a corpse prefab.
            GameObject corpse = (GameObject)Instantiate(Resources.Load("Corpse"));
            corpse.transform.position += transform.position;
            corpse.transform.rotation  = transform.rotation;
            corpse.transform.parent    = GameObject.Find("Entities").transform;
            corpse.name                = "Corpse";

            // Apply force to the corpse.
            Rigidbody corpseRigidbody = corpse.GetComponent<Rigidbody>();
            corpseRigidbody.AddForceAtPosition(force, position, ForceMode.Impulse);
        }
    }
}
