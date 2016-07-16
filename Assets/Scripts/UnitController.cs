using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour
{
    private Rigidbody m_rigidbody;

    public  float   m_maximumSpeed;
    private Vector3 m_currentVelocity;
    private Vector3 m_desiredVelocity;
    private Vector3 m_lookDirection;
    private bool    m_look;

    private bool    m_grounded;
    private bool    m_jump;
    public  float   m_jumpForce;

    private bool    m_shoot;
    private Vector3 m_shootDirection;
    public  Vector3 m_shootOrigin;
    private float   m_shootTimer;
    public  float   m_shootDelay;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    public void Look(Vector3 direction)
    {
        m_lookDirection = direction;
        m_look = true;
    }

    public void Move(Vector3 direction)
    {
        m_desiredVelocity = direction * m_maximumSpeed;
    }

    public void Jump()
    {
        m_jump = true;
    }

    public void Shoot(Vector3 direction)
    {
        m_shoot = true;
        m_shootDirection = direction;
    }

    public static float AngleSigned(Vector3 from, Vector3 to, Vector3 axis)
    {
        return Mathf.Atan2(Vector3.Dot(axis, Vector3.Cross(from, to)), Vector3.Dot(from, to)) * Mathf.Rad2Deg;
    }

    void FixedUpdate()
    {
        // Check if rigidbody is grounded.
        m_grounded = Physics.CheckSphere(transform.position + new Vector3(0.0f, 0.35f, 0.0f), 0.4f, ~(1 << LayerMask.NameToLayer("Capsule")));

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

        // Make the character jump.
        if(m_jump)
        {
            if(m_grounded)
            {
                m_rigidbody.AddForce(transform.up * m_jumpForce, ForceMode.VelocityChange);
            }

            m_jump = false;
        }

        // Update the desired facing direction.
        if(m_look == false)
        {
            if(m_desiredVelocity != Vector3.zero && m_rigidbody.velocity.magnitude >= 0.1f)
            {
                m_lookDirection = m_rigidbody.velocity.normalized;
            }
        }

        m_lookDirection.y = 0.0f;
        m_lookDirection.Normalize();

        m_look = false;

        // Update the rotation torque.
        float angleChange = AngleSigned(transform.forward, m_lookDirection, Vector3.up);
        Vector3 torqueChange = new Vector3(0.0f, angleChange, 0.0f) - m_rigidbody.angularVelocity;
        m_rigidbody.AddTorque(torqueChange, ForceMode.VelocityChange);

        // Make the character shoot.
        m_shootTimer = Mathf.Max(0.0f, m_shootTimer - Time.fixedDeltaTime);

        if(m_shoot)
        {
            if(m_shootTimer == 0.0f)
            {
                // Cast a ray from the shooting origin.
                Vector3 shootOriginWorld = transform.position + m_shootOrigin;

                RaycastHit hitResult;
                if(Physics.Raycast(shootOriginWorld, m_shootDirection, out hitResult, Mathf.Infinity))
                {
                    Debug.DrawLine(shootOriginWorld, hitResult.point, Color.red, 0.001f, false);

                    // Apply damage to hit entity.
                    if(hitResult.rigidbody)
                    {
                        HealthComponent health = hitResult.rigidbody.GetComponent<HealthComponent>();

                        if(health != null)
                        {
                            health.Damage(10, hitResult.point, -hitResult.normal * 300.0f);
                        }
                    } 
                }
                else
                {
                    Debug.DrawRay(shootOriginWorld, m_shootDirection * 100.0f, Color.red, 0.001f, false);
                }

                m_shootTimer = m_shootDelay;
            }

            m_shoot = false;
        }
    }

    void Update()
    {
        // Debug display.
        Debug.DrawRay(transform.position, m_rigidbody.velocity, Color.yellow, 0.001f, false);
        Debug.DrawRay(transform.position, transform.forward, Color.green, 0.001f, false);
    }
}
