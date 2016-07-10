using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public  GameObject       m_unit;
    private UnitController   m_unitController;
    public  CameraController m_cameraController;

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

        // Make the player character jump on key press.
        if(Input.GetKey(KeyCode.Space))
        {
            m_unitController.Jump();
        }

        // Make the player character fire on key press.
        if(Input.GetKey(KeyCode.Mouse0))
        {
            // Calculate the point at which we want to fire at for the 3rd person mode.
            Transform cameraTransform = m_cameraController.m_playerCamera.transform;

            RaycastHit hitResult;
            if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hitResult, Mathf.Infinity))
            {
                Vector3 shootSource = m_unit.transform.position + m_unitController.m_shootOrigin;
                m_unitController.Shoot(Vector3.Normalize(hitResult.point - shootSource));
            }
            else
            {
                Vector3 cameraTarget = cameraTransform.position + cameraTransform.forward * 100.0f;
                Vector3 shootSource = m_unit.transform.position + m_unitController.m_shootOrigin;
                m_unitController.Shoot(Vector3.Normalize(cameraTarget - shootSource));
            }
        }
    }
}
