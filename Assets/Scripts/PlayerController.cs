using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public GameObject m_unit;
    public CameraController m_cameraController;
    private UnitController m_unitController;

    void Start()
    {
        m_unitController = m_unit.GetComponent<UnitController>();
    }

    void Update()
    {
        // Calculate direction from user input.
        Vector3 direction = new Vector3();
        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");
        direction.Normalize();

        // Move unit in a direction relative to the camera.
        Transform cameraTransform = m_cameraController.m_camera.transform;
        m_unitController.MoveDirection(cameraTransform.rotation * direction);
    }
}
