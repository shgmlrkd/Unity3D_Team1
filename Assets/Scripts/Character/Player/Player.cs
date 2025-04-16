using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.Controls;
using TMPro;
using System.Collections.Generic;
using static Cinemachine.DocumentationSortingAttribute;
using System.Xml.Linq;

public class Player : MonoBehaviour
{
    Animator _anim;
    private Slider _healthSlider;
    private Slider _expSlider;
    private TextMeshProUGUI _hpText;
    private TextMeshProUGUI _expText;
    private float _maxHealth;
    private float _currentHealth;
    private PlayerData _playerData;
    private float _curExp;
    private int _playerLevel = 1;
    private float _offset = 1.2f;
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
        _healthSlider = GameObject.Find("PlayerHpBar").GetComponent<Slider>();
        _currentHealth = _maxHealth;
        Transform hpTextTransform = _healthSlider.transform.GetChild(2); // ← 0부터 시작, 3번째 자식은 index 2
        _hpText = hpTextTransform.GetComponent<TextMeshProUGUI>();

        _expSlider = GameObject.Find("PlayerExpBar").GetComponent<Slider>();
        _expSlider.value = 0;
        _curExp = 0;
        Transform expTextTransform = _expSlider.transform.GetChild(2); // ← 0부터 시작, 3번째 자식은 index 2
        _expText = expTextTransform.GetComponent<TextMeshProUGUI>();
        _expText.text = $"{(_curExp).ToString("F2")}%";
        if (_healthSlider != null)
        {
            _healthSlider.maxValue = _maxHealth;
            _healthSlider.value = _currentHealth;
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10f);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            GetExp(10);
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

            var data = PlayerDataManager.Instance.GetPlayerData(_playerLevel);
            if (_curExp >= data.Experience)
            {
                _playerLevel++;
                _curExp %= data.Experience;
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
            transform.position = newpos;
        }
        if (other.CompareTag("FrontWall")|| other.CompareTag("BackWall"))
        {
            Vector3 newpos = transform.position;
            newpos.z = -newpos.z;
            transform.position = newpos* _offset;
        }
    }
}
