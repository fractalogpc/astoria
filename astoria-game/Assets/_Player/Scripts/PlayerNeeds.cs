using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerNeeds : MonoBehaviour
{

    [SerializeField] private float _maxHunger = 100;
    [SerializeField] private float _maxThirst = 100;
    [SerializeField] private float _hungerRate = 1;
    [SerializeField] private float _thirstRate = 1;
    [SerializeField] private float _hungerDamageRate = 1;
    [SerializeField] private float _thirstDamageRate = 1;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _hungerText;
    [SerializeField] private TextMeshProUGUI _thirstText;
    [SerializeField] private Image _hungerBar;
    [SerializeField] private Image _thirstBar;

    [SerializeField] private HealthInterface _healthInterface;

    private float _currentHunger;
    private float _currentThirst;

    private void Start()
    {
        _currentHunger = _maxHunger;
        _currentThirst = _maxThirst;
    }

    public void AddHunger(float amount)
    {
        _currentHunger += amount;
        _currentHunger = Mathf.Clamp(_currentHunger, 0, _maxHunger);
        UpdateUI();
    }

    public void AddThirst(float amount)
    {
        _currentThirst += amount;
        _currentThirst = Mathf.Clamp(_currentThirst, 0, _maxThirst);
        UpdateUI();
    }

    private void UpdateUI()
    {
        _hungerText.text = $"{Mathf.CeilToInt(_currentHunger)} / {_maxHunger}";
        _thirstText.text = $"{Mathf.CeilToInt(_currentThirst)} / {_maxThirst}";

        _hungerBar.fillAmount = _currentHunger / _maxHunger;
        _thirstBar.fillAmount = _currentThirst / _maxThirst;
    }

    private void Update()
    {
        _currentHunger -= _hungerRate * Time.deltaTime;
        _currentThirst -= _thirstRate * Time.deltaTime;

        _currentHunger = Mathf.Clamp(_currentHunger, 0, _maxHunger);
        _currentThirst = Mathf.Clamp(_currentThirst, 0, _maxThirst);

        UpdateUI();

        if (_currentHunger <= 0)
        {
            _healthInterface.Damage(_hungerDamageRate * Time.deltaTime, transform.position);
        }

        if (_currentThirst <= 0)
        {
            _healthInterface.Damage(_thirstDamageRate * Time.deltaTime, transform.position);
        }
    }
}