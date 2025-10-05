using UnityEngine;

public class CannonController : MonoBehaviour
{
    [Header("Управление")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 90f;

    [Header("Снаряд")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private float _launchSpeed = 15f;

    [Header("Случайные параметры снаряда")]
    [SerializeField] private float _minMass = 0.5f;
    [SerializeField] private float _maxMass = 3f;
    [SerializeField] private float _minRadius = 0.05f;
    [SerializeField] private float _maxRadius = 0.3f;

    [Header("Физика")]
    [SerializeField] private float _dragCoefficient = 0.47f;
    [SerializeField] private float _airDensity = 1.225f;
    [SerializeField] private Vector3 _wind = Vector3.zero;

    private TrajectoryRenderer _trajectoryRenderer;
    private float _currentMass;
    private float _currentRadius;

    private void Awake()
    {
        _trajectoryRenderer = GetComponent<TrajectoryRenderer>();
        GenerateRandomProjectileParams();
    }

    private void Update()
    {
        HandleMovement();
        HandleShooting();
        UpdateTrajectory();
    }

    private void HandleMovement()
    {
        float moveX = 0;
        float moveZ = 0;

        if (Input.GetKey(KeyCode.W)) moveZ += 1;  
        if (Input.GetKey(KeyCode.S)) moveZ -= 1; 
        if (Input.GetKey(KeyCode.A)) moveX -= 1; 
        if (Input.GetKey(KeyCode.D)) moveX += 1; 

        Vector3 movement = (transform.right * moveX + transform.forward * moveZ) * _moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        float rotation = 0;
        if (Input.GetKey(KeyCode.E)) rotation = 1;
        else if (Input.GetKey(KeyCode.Q)) rotation = -1;

        transform.Rotate(0, rotation * _rotationSpeed * Time.deltaTime, 0);
    }
    private void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
            GenerateRandomProjectileParams();
        }
    }

    private void UpdateTrajectory()
    {
        if (_trajectoryRenderer != null && _muzzle != null)
        {
            _trajectoryRenderer.UpdateProjectileParams(_currentMass, _currentRadius);

            Vector3 startVelocity = _muzzle.forward * _launchSpeed;
            _trajectoryRenderer.DrawWithAirEuler(_muzzle.position, startVelocity);
        }
    }

    private void Fire()
    {
        if(_projectilePrefab == null || _muzzle == null) return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterShot();
        }

        GameObject projectile = Instantiate(_projectilePrefab, _muzzle.position, _muzzle.rotation);
        QuadraticDrag qd = projectile.GetComponent<QuadraticDrag>();

        if (qd != null)
        {
            Vector3 initialVelocity = _muzzle.forward * _launchSpeed;
            qd.SetPhysicalParams(_currentMass, _currentRadius, _dragCoefficient, _airDensity, _wind, initialVelocity);

            float scale = _currentRadius * 2f; 
            projectile.transform.localScale = Vector3.one * scale;
        }
    }


    private void GenerateRandomProjectileParams()
    {
        _currentMass = Random.Range(_minMass, _maxMass);
        _currentRadius = Random.Range(_minRadius, _maxRadius);
    }
}