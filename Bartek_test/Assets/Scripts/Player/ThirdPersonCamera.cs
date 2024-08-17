using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform m_target; // Obiekt, który kamera będzie śledzić
    [SerializeField] private float m_distance = 5.0f; // Odległość kamery od obiektu
    [SerializeField] private float m_zoomSpeed = 2.0f; // Szybkość zoomu
    [SerializeField] private float m_minDistance = 2.0f; // Minimalna odległość kamery
    [SerializeField] private float m_maxDistance = 10.0f; // Maksymalna odległość kamery
    [SerializeField] private float m_rotationSpeed = 150.0f; // Szybkość obrotu kamery
    [SerializeField] private float m_yMinLimit = -20f; // Minimalny kąt obrotu w osi Y
    [SerializeField] private float m_yMaxLimit = 80f; // Maksymalny kąt obrotu w osi Y

    private float m_currentX = 0.0f;
    private float m_currentY = 0.0f;

    void Update()
    {
        // Pobranie ruchu myszy z uwzględnieniem czasu rzeczywistego
        m_currentX += Input.GetAxis("Mouse X") * m_rotationSpeed * Time.deltaTime;
        m_currentY -= Input.GetAxis("Mouse Y") * m_rotationSpeed * Time.deltaTime;
        
        // Ograniczenie kąta obrotu kamery w osi Y
        m_currentY = Mathf.Clamp(m_currentY, m_yMinLimit, m_yMaxLimit);

        // Zoom kamery za pomocą kółka myszy
        m_distance -= Input.GetAxis("Mouse ScrollWheel") * m_zoomSpeed;
        m_distance = Mathf.Clamp(m_distance, m_minDistance, m_maxDistance);
    }

    void LateUpdate()
    {
        if (!m_target) return;

        // Obrót kamery wokół celu
        Quaternion rotation = Quaternion.Euler(m_currentY, m_currentX, 0);
        Vector3 direction = new Vector3(0, 0, -m_distance);
        Vector3 position = m_target.position + rotation * direction;

        // Ustawienie pozycji i rotacji kamery
        transform.position = position;
        transform.LookAt(m_target.position + Vector3.up * 1.5f); // Podniesienie punktu celowania, aby kamera patrzyła trochę ponad obiekt
    }
}