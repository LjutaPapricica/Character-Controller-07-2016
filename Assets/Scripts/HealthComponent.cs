using UnityEngine;
using System.Collections;

public class HealthComponent : MonoBehaviour
{
    private Renderer m_renderer;
    private float m_damagedTimer;

    public int m_currentHealth = 30;
    public int m_maximumHealth = 30;

    void Start()
    {
        // Get the renderer.
        m_renderer = transform.Find("Capsule").GetComponent<Renderer>();
    }

    void Update()
    {
        // Change emmisive color.
        Material material = m_renderer.material;
        material.SetColor("_EmissionColor", Color.red * m_damagedTimer * 2.0f);

        // Update the timer.
        m_damagedTimer = Mathf.Max(0.0f, m_damagedTimer - Time.deltaTime);
    }

    public void Heal(int health)
    {
        // Add health to the health pool.
        m_currentHealth = Mathf.Min(m_currentHealth + health, m_maximumHealth);
    }

    public void Damage(int health, Vector3 position, Vector3 force)
    {
        // Substract health from the health pool.
        m_currentHealth = Mathf.Max(m_currentHealth - health, 0);

        // Set damaged status.
        m_damagedTimer = 0.5f;

        // Check if the entity should be dead.
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
