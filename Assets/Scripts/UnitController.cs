using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour
{
    public float m_speed;

    private Rigidbody m_rigidbody;
    private Vector3 m_desiredVelocity;
    private Vector3 m_desiredDirection;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    public void MoveDirection(Vector3 direction)
    {
        m_desiredVelocity = direction * m_speed;
    }

    public static float AngleSigned(Vector3 from, Vector3 to, Vector3 axis)
    {
        return Mathf.Atan2(Vector3.Dot(axis, Vector3.Cross(from, to)), Vector3.Dot(from, to)) * Mathf.Rad2Deg;
    }

    void FixedUpdate()
    {
        // Update the velocity.
        if(m_desiredVelocity != Vector3.zero)
        {
            Vector3 velocityChange = m_desiredVelocity - m_rigidbody.velocity;
            velocityChange = Vector3.ClampMagnitude(velocityChange, 12.0f * Time.fixedDeltaTime);
            m_rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }

        // Update the desired facing direction.
        if(m_rigidbody.velocity.magnitude >= 0.1f)
        {
            m_desiredDirection = m_rigidbody.velocity.normalized;
        }

        // Update the rotation torque.
        float angleChange = AngleSigned(transform.forward, m_desiredDirection, Vector3.up);
        Vector3 torqueChange = new Vector3(0.0f, angleChange, 0.0f) - m_rigidbody.angularVelocity;
        m_rigidbody.AddTorque(torqueChange, ForceMode.VelocityChange);
    }

    void Update()
    {
        Debug.DrawRay(transform.position, m_rigidbody.velocity, Color.red);
        Debug.DrawRay(transform.position, transform.forward, Color.green);
    }
}
