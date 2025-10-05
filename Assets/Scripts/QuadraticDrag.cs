using UnityEngine;

public class QuadraticDrag : MonoBehaviour
{
    [Header("Параметры сопротивления")]
    [SerializeField] private float _mass = 1f;
    [SerializeField] private float _radius = 0.1f;
    [SerializeField] private float _dragCoefficient = 0.47f;
    [SerializeField] private float _airDensity = 1.225f;
    [SerializeField] private Vector3 _wind = Vector3.zero;

    private Rigidbody _rb;
    private float _area;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _area = Mathf.PI * _radius * _radius;
    }

    private void FixedUpdate()
    {
        Vector3 vRel = _rb.linearVelocity - _wind;
        float speed = vRel.magnitude;
        if (speed < 1e-6f) return;

        Vector3 drag = -0.5f * _airDensity * _dragCoefficient * _area * speed * vRel;
        _rb.AddForce(drag, ForceMode.Force);
    }

    public void SetPhysicalParams(float mass, float radius, float dragCoefficient, float airDensity, Vector3 wind, Vector3 initialVelocity)
    {
        _mass = Mathf.Max(0.001f, mass);
        _radius = Mathf.Max(0.001f, radius);
        _dragCoefficient = Mathf.Max(0f, dragCoefficient);
        _airDensity = Mathf.Max(0f, airDensity);
        _wind = wind;

        _rb.mass = _mass;
        _rb.linearDamping = 0f;
        _rb.useGravity = true;
        _rb.linearVelocity = initialVelocity;
        _area = Mathf.PI * _radius * _radius;
    }
}