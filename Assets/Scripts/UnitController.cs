using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour
{
    public float m_speed;

    private Rigidbody m_rigidbody;
    private Vector3 m_cappedVelocity;
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
        // Check if rigidbody is grounded.
        bool grounded = Physics.CheckSphere(transform.position + new Vector3(0.0f, 0.3f, 0.0f), 0.4f, ~(1 << LayerMask.NameToLayer("Capsule")));

        // Update current velocity.
        if(grounded)
        {
            Vector3 velocityChange = m_desiredVelocity - m_cappedVelocity;

            if(m_desiredVelocity != Vector3.zero)
            {
                m_cappedVelocity += Vector3.ClampMagnitude(velocityChange, 48.0f * Time.fixedDeltaTime);
            }
            else
            {
                m_cappedVelocity += Vector3.ClampMagnitude(velocityChange, 8.0f * Time.fixedDeltaTime);
            }

            m_rigidbody.AddForce(m_cappedVelocity - m_rigidbody.velocity, ForceMode.VelocityChange);
        }

        // Update the desired facing direction.
        if(m_desiredVelocity != Vector3.zero && m_rigidbody.velocity.magnitude >= 0.1f)
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
        // Debug display.
        Debug.DrawRay(transform.position, m_rigidbody.velocity, Color.red, 0.0001f, false);
        Debug.DrawRay(transform.position, transform.forward, Color.green, 0.0001f, false);
    }
}
