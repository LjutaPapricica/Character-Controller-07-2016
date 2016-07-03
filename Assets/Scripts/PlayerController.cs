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

        // Calculate camera rotation around y-axis.
        Quaternion cameraRotation = Quaternion.Euler(0.0f, m_cameraController.m_rotation.y, 0.0f);

        // Move the controlled unit in a direction relative to the camera.
        m_unitController.Move(cameraRotation * direction);

        // Handle user input.
        if(Input.GetKey(KeyCode.Space))
        {
            m_unitController.Jump();
        }

        if(Input.GetKey(KeyCode.Mouse0))
        {
            m_unitController.Shoot(m_cameraController.m_camera.transform.forward);
        }
    }
}
