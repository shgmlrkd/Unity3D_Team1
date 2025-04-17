using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class PlayerHpExp : MonoBehaviour
{
    private Slider _healthSlider;
    private Slider _expSlider;
    private TextMeshProUGUI _hpText;
    private TextMeshProUGUI _expText;
    private PlayerData _playerData;
    private float _maxHealth;
    private float _currentHealth;
    private float _curExp;
    private int _playerLevel = 1;
    private Renderer[] _renderers;
    private RawImage _deathScreen;
    private bool _isFadingOut = true;
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
        _renderers = GetComponentsInChildren<Renderer>();

        GameObject obj = GameObject.Find("DeathScreen");
        if (obj != null)
        {
            _deathScreen = obj.GetComponent<RawImage>();

            if (_deathScreen != null)
            {
                
                Color c = _deathScreen.color;
                c.a = 0.0f;
                _deathScreen.color = c;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10f);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(10000f);
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

        StartCoroutine(MakeRed());
        if (_currentHealth <= 0f && _isFadingOut)
        {
            StartCoroutine(FadeInDeathScreen(2.0f)); // 1초 동안 서서히 표시
            _isFadingOut = false;
        }
    }
    private IEnumerator FadeInDeathScreen(float duration)
    {
        float timer = 0f;
        Color c = _deathScreen.color;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / duration);
            c.a = alpha;
            _deathScreen.color = c;
            yield return null;
        }

        c.a = 1f;
        _deathScreen.color = c;
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

    private IEnumerator MakeRed()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        foreach (Renderer render in _renderers)
        {
            render.GetPropertyBlock(block);
            block.SetColor("_BaseColor", Color.red);
            render.SetPropertyBlock(block);
        }

        yield return new WaitForSeconds(0.1f);

        foreach (Renderer render in _renderers)
        {
            render.GetPropertyBlock(block);
            block.SetColor("_BaseColor", Color.white); // 또는 원래 색상 저장했을 경우 그걸로 복원
            render.SetPropertyBlock(block);
        }
    }
}
