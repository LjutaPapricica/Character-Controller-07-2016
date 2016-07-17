using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour
{
    public bool m_enabled = true;

    private UnitController m_unitController;
    private Collider m_collider;

    void Start()
    {
        // Get required components.
        m_unitController = GetComponent<UnitController>();
        m_collider = GetComponentInChildren<Collider>();
    }

    void FixedUpdate()
    {
        if(!m_enabled)
            return;

        // Get a list of all colliders in a radius.
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Capsule");
        Collider[] colldiers = Physics.OverlapSphere(transform.position, 8.0f, layerMask);

        // Get the closest collider.
        GameObject closestCollider = null;
        float closestDistanceSqr = float.MaxValue;

        foreach(Collider colldier in colldiers)
        {
            // Ignore self.
            if(colldier == m_collider)
                continue;

            // Check distance to a collider.
            float distanceSqr = (colldier.transform.position - transform.position).sqrMagnitude;

            if(distanceSqr < closestDistanceSqr)
            {
                closestCollider = colldier.gameObject;
                closestDistanceSqr = distanceSqr;
            }
        }

        // Look at the direction of the closest unit.
        if(closestCollider != null)
        {
            Vector3 offset = closestCollider.transform.position - m_collider.transform.position;

            m_unitController.Shoot(offset.normalized);
        }
    }
}
