using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.Controls;
using TMPro;

public class Player : MonoBehaviour
{
    Animator anim;
    private Slider healthSlider;
    public TextMeshProUGUI hpText;
    private float maxHealth = 100f;
    private float currentHealth;
    
    public enum State
    {
        Idle,
        Walking,
        Running,
        Dead
    }

    private static Player _instance;
    public static Player Instance
    {
        get { return _instance; }
    }

    private float _playerMoveSpeed = 6.0f;
    private float _playerRotateSpeed = 12.0f;

    private bool _isRunning = false;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        healthSlider = GameObject.Find("PlayerHpBar").GetComponent<Slider>();
        currentHealth = maxHealth;
        Transform textTransform = healthSlider.transform.GetChild(2); // ← 0부터 시작, 3번째 자식은 index 2
        hpText = textTransform.GetComponent<TextMeshProUGUI>();

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (inputDir.sqrMagnitude > 0)
        {
            _isRunning = true;
            transform.Translate(inputDir.normalized * _playerMoveSpeed * Time.deltaTime, Space.World); //position += inputDir * playerMoveSpeed * Time.deltaTime;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(inputDir), Time.deltaTime * _playerRotateSpeed);
        }
        else
        {
            _isRunning = false;
        }

        anim.SetBool("IsRunning", _isRunning);

        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10f);
        }
    }
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (hpText != null)
            hpText.text = $"{currentHealth} / {maxHealth}";

        UpdateUI();
    }
    private void UpdateUI()
    {

    }
}

