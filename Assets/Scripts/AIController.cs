using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour
{
    public bool m_enabled = true;

    private UnitController  m_unitController;
    private CapsuleCollider m_collider;

    void Start()
    {
        // Get required components.
        m_unitController = GetComponent<UnitController>();
        m_collider = GetComponentInChildren<CapsuleCollider>();
    }

    void FixedUpdate()
    {
        if(!m_enabled)
            return;

        // Get a list of all colliders in a radius.
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Capsule");
        Collider[] colldiers = Physics.OverlapSphere(transform.position, 32.0f, layerMask);

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

            if(closestDistanceSqr > 8.0f * 8.0f)
            {
                m_unitController.Move(offset.normalized);
            }
            else
            {
                m_unitController.Shoot(offset.normalized);
            }
        }
    }
}
