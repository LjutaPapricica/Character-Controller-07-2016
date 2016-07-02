using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Camera m_camera;
    public PlayerController m_playerController;
    public Vector3 m_offset;
    private Vector3 m_rotation;

	void Start()
    {
	}

	void Update()
    {
        Transform cameraTransform = m_camera.transform;
        Transform playerTransform = m_playerController.m_unit.transform;

        // Calculate rotation from user input.
        m_rotation.y += Input.GetAxis("Mouse X");
        m_rotation.x += Input.GetAxis("Mouse Y");

        // Wrap rotation values.
        m_rotation.y = Mathf.Repeat(m_rotation.y, 360.0f);
        m_rotation.x = Mathf.Repeat(m_rotation.x, 360.0f);

        // Update camera rotation.
        cameraTransform.rotation = Quaternion.identity;
        cameraTransform.rotation *= Quaternion.Euler(Vector3.up * m_rotation.y);
        cameraTransform.rotation *= Quaternion.Euler(Vector3.left * m_rotation.x);

        // Update camera position.
        cameraTransform.position = playerTransform.position + cameraTransform.rotation * m_offset;
    }
}
