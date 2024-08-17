using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed = 5.0f; // Szybkość poruszania się postaci
    [SerializeField] private float m_jumpForce = 5.0f; // Siła skoku
    [SerializeField] private Animator m_animator; // Animator przypisany do postaci
    [SerializeField] private LayerMask m_groundLayers; // Warstwa terenu, z którym postać może kolidować

    private Rigidbody m_rigidbody;
    private CapsuleCollider m_collider;
    private Vector3 m_movement;
    private bool m_isGrounded;

    void Start()
    {
        // Pobranie komponentów Rigidbody i CapsuleCollider
        m_rigidbody = GetComponent<Rigidbody>();
        m_collider = GetComponent<CapsuleCollider>();

        // Wyłączenie rotacji w osi X i Z, aby postać się nie przewracała
        m_rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // Pobranie wartości wejściowych (klawisze WSAD / Strzałki)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Ustawienie kierunku ruchu
        m_movement = new Vector3(horizontal, 0f, vertical).normalized;

        // Sprawdzenie, czy postać się porusza
        bool isRunning = m_movement.magnitude > 0f;
        m_animator.SetBool("Run", isRunning);

        // Sprawdzenie, czy postać dotyka ziemi
        m_isGrounded = CheckGrounded();

        // Skok
        if (Input.GetButtonDown("Jump") && m_isGrounded)
        {
            m_rigidbody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        if (m_movement.magnitude > 0f)
        {
            // Przemieszczenie postaci
            Vector3 move = transform.forward * m_movement.magnitude * m_moveSpeed * Time.fixedDeltaTime;
            m_rigidbody.MovePosition(m_rigidbody.position + move);

            // Obrócenie postaci w kierunku ruchu
            Quaternion targetRotation = Quaternion.LookRotation(m_movement);
            m_rigidbody.MoveRotation(targetRotation);
        }
    }

    private bool CheckGrounded()
    {
        // Sprawdzanie, czy postać dotyka ziemi za pomocą promienia wychodzącego z dolnej części CapsuleCollidera
        float capsuleBottom = m_collider.bounds.center.y - m_collider.bounds.extents.y;
        bool grounded = Physics.CheckCapsule(m_collider.bounds.center,
            new Vector3(m_collider.bounds.center.x, capsuleBottom, m_collider.bounds.center.z),
            m_collider.radius * 0.9f,
            m_groundLayers);
        return grounded;
    }
}