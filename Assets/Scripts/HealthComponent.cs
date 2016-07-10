using UnityEngine;
using System.Collections;

public class HealthComponent : MonoBehaviour
{
    public int currentHealth = 30;
    public int maximumHealth = 30;

    public void Heal(int health)
    {
        this.currentHealth = Mathf.Min(this.currentHealth + health, this.maximumHealth);
    }

    public void Damage(int health, Vector3 position, Vector3 force)
    {
        this.currentHealth = Mathf.Max(this.currentHealth - health, 0);

        if(this.currentHealth == 0)
        {
            // Disable collisition of the object.
            Rigidbody rigidbody = this.GetComponent<Rigidbody>();
            rigidbody.detectCollisions = false;

            // Destroy the object.
            Destroy(this.gameObject);

            // Create a corpse prefab.
            GameObject corpse = (GameObject)Instantiate(Resources.Load("Corpse"));
            corpse.transform.position += this.transform.position;
            corpse.transform.rotation  = this.transform.rotation;
            corpse.transform.parent    = GameObject.Find("Entities").transform;
            corpse.name                = "Corpse";

            // Apply force to the corpse.
            Rigidbody corpseRigidbody = corpse.GetComponent<Rigidbody>();
            corpseRigidbody.AddForceAtPosition(force, position);
        }
    }
}
