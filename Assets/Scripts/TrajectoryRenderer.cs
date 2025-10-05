using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryRenderer : MonoBehaviour
{
    [Header("Отрисовка")]
    [SerializeField] private int _pointsCount = 50;
    [SerializeField] private float _timeStep = 0.02f;
    [SerializeField] private float _lineWidth = 0.02f;

    [Header("Физика воздуха")]
    [SerializeField] private float _dragCoefficient = 0.47f;
    [SerializeField] private float _airDensity = 1.225f;
    [SerializeField] private Vector3 _wind = Vector3.zero;

    private float _mass = 1f;
    private float _radius = 0.1f;
    private float _area;
    private LineRenderer _line;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _line.useWorldSpace = true;
        _line.widthMultiplier = _lineWidth;
        _line.material = new Material(Shader.Find("Sprites/Default"));
        _line.material.color = Color.red;
        UpdateArea();
    }

    public void UpdateProjectileParams(float mass, float radius)
    {
        _mass = mass;
        _radius = radius;
        UpdateArea();
    }

    private void UpdateArea()
    {
        _area = Mathf.PI * _radius * _radius;
    }

    public void DrawWithAirEuler(Vector3 startPosition, Vector3 startVelocity)
    {
        if (_pointsCount < 2) _pointsCount = 2;

        Vector3 p = startPosition;
        Vector3 v = startVelocity;
        _line.positionCount = _pointsCount;

        for (int i = 0; i < _pointsCount; i++)
        {
            _line.SetPosition(i, p);

            Vector3 vRel = v - _wind;
            float speed = vRel.magnitude;
            Vector3 drag = speed > 1e-6f ?
                (-0.5f * _airDensity * _dragCoefficient * _area * speed) * vRel :
                Vector3.zero;

            Vector3 a = Physics.gravity + drag / _mass;
            v += a * _timeStep;
            p += v * _timeStep;

            if (p.y < 0) break;
        }
    }

    public void DrawVacuum(Vector3 startPosition, Vector3 startVelocity)
    {
        if (_pointsCount < 2) _pointsCount = 2;
        _line.positionCount = _pointsCount;

        for (int i = 0; i < _pointsCount; i++)
        {
            float t = i * _timeStep;
            Vector3 p = startPosition + startVelocity * t + 0.5f * Physics.gravity * t * t;
            _line.SetPosition(i, p);

            if (p.y < 0) break;
        }
    }
}