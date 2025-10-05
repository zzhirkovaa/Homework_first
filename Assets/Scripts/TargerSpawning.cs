using UnityEngine;
using System.Collections.Generic;

public class TargetSpawner : MonoBehaviour
{
    [Header("Мишени")]
    [SerializeField] private GameObject _targetPrefab;
    [SerializeField] private int _maxTargets = 5;
    [SerializeField] private float _spawnInterval = 3f;

    [Header("Зона спавна")]
    [SerializeField] private float _spawnRadius = 10f;
    [SerializeField] private float _minHeight = 3f;
    [SerializeField] private float _maxHeight = 8f;

    [Header("Случайные параметры мишеней")]
    [SerializeField] private float _minMass = 0.3f;
    [SerializeField] private float _maxMass = 2f;
    [SerializeField] private float _minRadius = 0.2f;
    [SerializeField] private float _maxRadius = 0.8f;
    [SerializeField] private float _minSpeed = 2f;
    [SerializeField] private float _maxSpeed = 6f;

    private List<GameObject> _activeTargets = new List<GameObject>();
    private float _spawnTimer;

    private void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= _spawnInterval && _activeTargets.Count < _maxTargets)
        {
            SpawnTarget();
            _spawnTimer = 0f;
        }

        _activeTargets.RemoveAll(target => target == null);
    }

    private void SpawnTarget()
    {
        if (_targetPrefab == null) return;

        Vector3 spawnPos = transform.position + Random.insideUnitSphere * _spawnRadius;
        spawnPos.y = Random.Range(_minHeight, _maxHeight);

        GameObject target = Instantiate(_targetPrefab, spawnPos, Random.rotation);
        _activeTargets.Add(target);

        float mass = Random.Range(_minMass, _maxMass);
        float radius = Random.Range(_minRadius, _maxRadius);
        float speed = Random.Range(_minSpeed, _maxSpeed);

        TargetController targetController = target.GetComponent<TargetController>();
        if (targetController != null)
        {
            targetController.Initialize(mass, radius, speed);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _spawnRadius);
    }
}