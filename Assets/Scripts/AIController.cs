using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour
{
    public bool m_enabled = true;
    private UnitController m_unitController;

    void Start()
    {
        // Get required components.
        m_unitController = GetComponent<UnitController>();
    }

    void FixedUpdate()
    {
        if(!m_enabled)
            return;

        // Get the player controller.
        PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        // Look at the direction of the player.
        Vector3 offset = playerController.m_unit.transform.position - transform.position;

        m_unitController.Look(offset.normalized);
    }
}
