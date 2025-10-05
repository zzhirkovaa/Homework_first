using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _infoText;

    private int _score = 0;
    private int _shotsFired = 0;
    private int _hits = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        UpdateUI();
    }

    public void AddScore(int value)
    {
        _score += value;
        _hits++;
    }

    public void RegisterShot()
    {
        _shotsFired++;
    }

    private void UpdateUI()
    {
        if (_scoreText != null)
            _scoreText.text = $"Очки: {_score}";

        if (_infoText != null)
        {
            float accuracy = _shotsFired > 0 ? (float)_hits / _shotsFired * 100 : 0;
            _infoText.text = $"Выстрелы: {_shotsFired}\nПопадания: {_hits}\nТочность: {accuracy:F1}%";
        }
    }

    public void ShowGameSummary()
    {
        float accuracy = _shotsFired > 0 ? (float)_hits / _shotsFired * 100 : 0;
    }
}