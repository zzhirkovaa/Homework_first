using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private Text _scoreText;

    private int _score = 0;

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
    }

    private void UpdateUI()
    {
        if (_scoreText != null)
            _scoreText.text = $"Очки: {_score}";
    }
}