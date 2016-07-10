using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Camera           m_playerCamera;
    public PlayerController m_playerController;

    public Vector3 m_anchor;
    public Vector3 m_offset;
    public Vector3 m_rotation;

	void Update()
    {
        // Get object transforms.
        Transform cameraTransform = m_playerCamera.transform;
        Transform playerTransform = m_playerController.m_unit.transform;

        // Calculate rotation from user input.
        m_rotation.y = Mathf.Repeat(m_rotation.y + Input.GetAxis("Mouse X"), 360.0f);
        m_rotation.x = Mathf.Repeat(m_rotation.x + Input.GetAxis("Mouse Y"), 360.0f);

        // Update camera rotation.
        cameraTransform.rotation  = Quaternion.identity;
        cameraTransform.rotation *= Quaternion.Euler(Vector3.up * m_rotation.y);
        cameraTransform.rotation *= Quaternion.Euler(Vector3.left * m_rotation.x);

        // Update camera position.
        cameraTransform.position  = playerTransform.position;
        cameraTransform.position += m_anchor;
        cameraTransform.position += cameraTransform.rotation * m_offset;
    }
}
