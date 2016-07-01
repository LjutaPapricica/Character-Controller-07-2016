using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour
{
    public float m_speed;

    private Rigidbody m_rigidbody;
    private Vector3 m_desiredVelocity;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    public void MoveDirection(Vector3 direction)
    {
        m_desiredVelocity = direction * m_speed;
    }

    void FixedUpdate()
    {
        if(m_desiredVelocity != Vector3.zero)
        {
            Vector3 velocityChange = m_desiredVelocity - m_rigidbody.velocity;
            velocityChange = Vector3.ClampMagnitude(velocityChange, 12.0f * Time.fixedDeltaTime);
            m_rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }

    void Update()
    {
        Debug.DrawRay(transform.position, m_rigidbody.velocity, Color.red);
        Debug.DrawRay(transform.position, transform.forward, Color.green);
    }
}
