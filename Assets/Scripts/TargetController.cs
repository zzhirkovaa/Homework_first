using UnityEngine;

public class TargetController : MonoBehaviour
{
    [Header("Статистика")]
    [SerializeField] private int _scoreValue = 1;

    private Rigidbody _rb;
    private float _mass;
    private float _radius;
    private float _speed;
    private bool _isInitialized = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public void Initialize(float mass, float radius, float speed)
    {
        _mass = mass;
        _radius = radius;
        _speed = speed;
        _rb.mass = _mass;
        _rb.useGravity = false; 

        transform.localScale = Vector3.one * _radius * 2f;

        Vector3[] horizontalDirections = {
        new Vector3(1, 0, 0),   
        new Vector3(-1, 0, 0),  
        new Vector3(0, 0, 1),  
        new Vector3(0, 0, -1) 
    };

        Vector3 randomDirection = horizontalDirections[Random.Range(0, horizontalDirections.Length)];
        _rb.linearVelocity = randomDirection * _speed;

        _isInitialized = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(_scoreValue);
            }
            Destroy(gameObject);
        }
    }

}