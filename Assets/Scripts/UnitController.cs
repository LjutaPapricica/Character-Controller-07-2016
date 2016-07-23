using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour
{
    private Rigidbody       m_rigidbody;
    private CapsuleCollider m_collider;

    public  float   m_movementSpeed;
    private Vector3 m_movementVelocity;

    private bool    m_look;
    private Vector3 m_lookDirection;

    private bool    m_grounded;
    private float   m_groundedTimer;

    private bool    m_jump;
    public  float   m_jumpHeight;
    public  float   m_jumpCooldown;
    private float   m_jumpTimer;
    public float    m_airControl;

    private bool    m_shoot;
    private Vector3 m_shootDirection;
    public  Vector3 m_shootOrigin;
    private float   m_shootTimer;
    public  float   m_shootDelay;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_collider = GetComponentInChildren<CapsuleCollider>();

        m_lookDirection = transform.forward;
    }

    public void Look(Vector3 direction)
    {
        m_look = true;
        m_lookDirection = direction;
    }

    public void Move(Vector3 direction)
    {
        m_movementVelocity = direction * m_movementSpeed;
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

    void Update()
    {
        // Debug display.
        Debug.DrawRay(transform.position, m_rigidbody.velocity, Color.yellow, 0.001f, false);
        Debug.DrawRay(transform.position, transform.forward, Color.green, 0.001f, false);
    }

    void FixedUpdate()
    {
        // Update grounded flag timer.
        m_groundedTimer = Mathf.Max(0.0f, m_groundedTimer - Time.fixedDeltaTime);

        // Calculate slope rotation.
        Quaternion slopeRotation = Quaternion.identity;
        Quaternion slopeRotationInv = Quaternion.identity;

        if(m_grounded)
        {
            RaycastHit raycastGround;
            Physics.Raycast(new Ray(transform.position + new Vector3(0.0f, 0.5f, 0.0f), -transform.up), out raycastGround);

            slopeRotation = Quaternion.FromToRotation(Vector3.up, raycastGround.normal);
            slopeRotationInv = Quaternion.Inverse(slopeRotation);
        }

        // Calculate planar velocity.
        Vector3 planarVelocity = m_rigidbody.velocity;
        planarVelocity = slopeRotationInv * planarVelocity;
        planarVelocity.y = 0.0f;
        planarVelocity = slopeRotation * planarVelocity;

        // Update movement velocity.
        if(m_movementVelocity != Vector3.zero)
        {
            Vector3 velocityChange = slopeRotation * m_movementVelocity - planarVelocity;

            if(!m_grounded)
            {
                velocityChange = Vector3.ClampMagnitude(velocityChange, m_airControl * Time.fixedDeltaTime);
            }

            m_rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }

        // Update jump velocity.
        m_jumpTimer = Mathf.Max(0.0f, m_jumpTimer - Time.fixedDeltaTime);

        if(m_jump)
        {
            if(m_grounded && m_jumpTimer == 0.0f)
            {
                float jumpForce = Mathf.Sqrt(2.0f * 2.0f * Mathf.Abs(Physics.gravity.y));
                m_rigidbody.AddForce(new Vector3(0.0f, jumpForce, 0.0f), ForceMode.VelocityChange);

                m_jumpTimer = m_jumpCooldown;

                // Set a grounded timer to prevent grounded state from being
                // applied again before the collider lifts from the ground.
                m_groundedTimer = 0.1f;
            }

            m_jump = false;
        }

        // Reset grounded flag.
        m_grounded = false;

        // Update the desired facing direction.
        if(m_look == false)
        {
            if(m_movementVelocity != Vector3.zero && m_rigidbody.velocity.magnitude >= 0.1f)
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
                            health.Damage(10, hitResult.point, -hitResult.normal * 10.0f);
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

    void OnCollisionStay(Collision collision)
    {
        TrackGrounded(collision);
    }

    void OnCollisionEnter(Collision collision)
    {
        TrackGrounded(collision);
    }

    void TrackGrounded(Collision collision)
    {
        // Calculate the minimum contact point for the character to be considered grounded.
        float maxHeight = m_collider.bounds.min.y + m_collider.radius * 0.9f;

        //
        foreach(ContactPoint contact in collision.contacts)
        {
            if(contact.point.y < maxHeight)
            {
                if(!IsStatic(collision) && !IsKinematic(collision))
                {
                    //m_currentVelocity = Vector3.zero;
                }

                if(m_groundedTimer == 0.0f)
                {
                    m_grounded = true;
                }
            }

            break;
        }
    }

    bool IsKinematic(Collision collision)
    {
        return IsKinematic(collision.transform);
    }

    bool IsKinematic(Transform transform)
    {
        Rigidbody rigidbody = transform.GetComponent<Rigidbody>();
        return rigidbody != null && rigidbody.isKinematic;
    }

    bool IsStatic(Collision collision)
    {
        return IsStatic(collision.transform);
    }

    bool IsStatic(Transform transform)
    {
        return transform.gameObject.isStatic;
    }
}
