using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Camera m_camera;
    public PlayerController m_playerController;
    public Vector3 m_offset;
    private float m_angle;

	void Start()
    {
	}

	void Update()
    {
        Transform cameraTransform = m_camera.transform;
        Transform playerTransform = m_playerController.m_unit.transform;
        
        // Update camera rotation.
        cameraTransform.rotation *= Quaternion.Euler(Vector3.up * Input.GetAxis("Mouse X"));

        // Update camera position.
        cameraTransform.position = playerTransform.position + cameraTransform.rotation * m_offset;
    }
}
