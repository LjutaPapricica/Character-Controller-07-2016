using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Camera           playerCamera;
    public PlayerController playerController;

    public Vector3 anchor;
    public Vector3 offset;
    public Vector3 rotation;

	void Update()
    {
        // Get object transforms.
        Transform cameraTransform = this.playerCamera.transform;
        Transform playerTransform = this.playerController.unit.transform;

        // Calculate rotation from user input.
        this.rotation.y = Mathf.Repeat(this.rotation.y + Input.GetAxis("Mouse X"), 360.0f);
        this.rotation.x = Mathf.Repeat(this.rotation.x + Input.GetAxis("Mouse Y"), 360.0f);

        // Update camera rotation.
        cameraTransform.rotation  = Quaternion.identity;
        cameraTransform.rotation *= Quaternion.Euler(Vector3.up * this.rotation.y);
        cameraTransform.rotation *= Quaternion.Euler(Vector3.left * this.rotation.x);

        // Update camera position.
        cameraTransform.position  = playerTransform.position;
        cameraTransform.position += this.anchor;
        cameraTransform.position += cameraTransform.rotation * this.offset;
    }
}
