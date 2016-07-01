using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public GameObject m_player;
    private UnitController m_controller;

    void Start()
    {
        m_controller = m_player.GetComponent<UnitController>();
    }

    void Update()
    {
        Vector3 direction = new Vector3();
        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");
        direction.Normalize();

        m_controller.MoveDirection(direction);
    }
}
