using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour
{
    public float m_speed;

    private Rigidbody m_rigidbody;
    private Vector3 m_currentVelocity;
    private Vector3 m_desiredVelocity;
    private Vector3 m_desiredDirection;

    private bool m_grounded;
    private bool m_jump;

    public Vector3 m_shootOrigin;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction)
    {
        m_desiredVelocity = direction * m_speed;
    }

    public void Jump()
    {
        m_jump = true;
    }

    public void Shoot(Vector3 direction)
    {
        Debug.DrawRay(transform.position + m_shootOrigin, direction * 100.0f, Color.red, 0.0001f, false);
    }

    public static float AngleSigned(Vector3 from, Vector3 to, Vector3 axis)
    {
        return Mathf.Atan2(Vector3.Dot(axis, Vector3.Cross(from, to)), Vector3.Dot(from, to)) * Mathf.Rad2Deg;
    }

    void FixedUpdate()
    {
        // Check if rigidbody is grounded.
        m_grounded = Physics.CheckSphere(transform.position + new Vector3(0.0f, 0.3f, 0.0f), 0.4f, ~(1 << LayerMask.NameToLayer("Capsule")));

        // Make the character jump.
        if(m_grounded && m_jump)
        {
            m_rigidbody.AddForce(transform.up * 4.0f, ForceMode.VelocityChange);

            m_grounded = false;
            m_jump = false;
        }

        // Update current velocity.
        if(m_grounded)
        {
            Vector3 velocityChange = m_desiredVelocity - m_currentVelocity;

            if(m_desiredVelocity != Vector3.zero)
            {
                m_currentVelocity += Vector3.ClampMagnitude(velocityChange, 48.0f * Time.fixedDeltaTime);
            }
            else
            {
                m_currentVelocity += Vector3.ClampMagnitude(velocityChange, 8.0f * Time.fixedDeltaTime);
            }

            m_rigidbody.AddForce(m_currentVelocity - m_rigidbody.velocity, ForceMode.VelocityChange);
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
        Debug.DrawRay(transform.position, m_rigidbody.velocity, Color.yellow, 0.0001f, false);
        Debug.DrawRay(transform.position, transform.forward, Color.green, 0.0001f, false);
    }
}
