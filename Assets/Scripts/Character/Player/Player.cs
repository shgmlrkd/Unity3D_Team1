using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Player : MonoBehaviour
{
    private Slider _healthSlider;
    private Slider _expSlider;
    private TextMeshProUGUI _hpText;
    private TextMeshProUGUI _expText;

    private List<BulletSkill> _bulletSkills = new List<BulletSkill>();

    private PlayerData _playerData;

    private float _maxHealth;
    private float _currentHealth;
    private float _curExp;
    private float _offset = 1.2f;
    private int _playerLevel = 1;

    private static Player _instance;
    public static Player Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        _playerData = PlayerDataManager.Instance.GetPlayerData(1);

        _maxHealth = _playerData.Hp;
        _currentHealth = _maxHealth;

        _curExp = 0;

        _healthSlider = GameObject.Find("PlayerHpBar").GetComponent<Slider>();
        Transform hpTextTransform = _healthSlider.transform.GetChild(2); // ← 0부터 시작, 3번째 자식은 index 2
        _hpText = hpTextTransform.GetComponent<TextMeshProUGUI>();

        _expSlider = GameObject.Find("PlayerExpBar").GetComponent<Slider>();
        Transform expTextTransform = _expSlider.transform.GetChild(2); // ← 0부터 시작, 3번째 자식은 index 2
        _expText = expTextTransform.GetComponent<TextMeshProUGUI>();

        _expSlider.value = 0;
        _expText.text = $"{(_curExp).ToString("F2")}%";

        if (_healthSlider != null)
        {
            _healthSlider.maxValue = _maxHealth;
            _healthSlider.value = _currentHealth;
        }

        _bulletSkills.Add(gameObject.AddComponent<BulletSkill>());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10f);
        }
    }
    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        if (_healthSlider != null)
            _healthSlider.value = _currentHealth;

        if (_hpText != null)
            _hpText.text = $"{_currentHealth} / {_maxHealth}";

        UpdateUI();
    }

    public void GetExp(int exp)
    {
        _curExp += exp;

        PlayerData data = PlayerDataManager.Instance.GetPlayerData(_playerLevel);
        if (_curExp >= data.Experience)
        {
            _playerLevel++;
            _curExp %= data.Experience;
            gameObject.GetComponent<PlayerMove>().PlayerSpeedUp(_playerLevel);
            Debug.Log($"Level Up! Now Level {_playerLevel}");
        }

        _expText.text = $"{(_curExp / data.Experience * 100f).ToString("F2")}%";
        _expSlider.value = _curExp / data.Experience;
    }

    private void UpdateUI()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LeftWall")|| other.CompareTag("RightWall"))
        {
            Vector3 newpos = transform.position;
            newpos.x = -newpos.x;

            if (newpos.x < 0)
            {
                newpos.x += _offset;
            }
            else
            {
               newpos.x -= _offset;
            }

            transform.position = newpos;
        }
        if (other.CompareTag("FrontWall")|| other.CompareTag("BackWall"))
        {
            Vector3 newpos = transform.position;
            newpos.z = -newpos.z;

            if (newpos.z < 0)
            {
                newpos.z += _offset;
            }
            else
            {
                newpos.z -= _offset;
            }

            transform.position = newpos;
        }
    }
}
