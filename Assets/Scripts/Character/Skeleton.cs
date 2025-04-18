using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Skeleton : MonoBehaviour
{
    private Transform _player;
    private GameObject _skeletonHpBarPrefab;
    private Slider _skeletonHpBarSlider;
    private Animator _animator;
    private Collider _skeletonCollider;
    private PlayerHpExp _playerHpExp;

    private Image _hpFillImage;
    private Image _hpBLImage;
    private Image _hpBGImage;

    private Vector3 _skeletonHpBarOffset;
    private MonsterData _monsterData;

    private float _skeletonRotateSpeed = 5.0f;
    private float _attackTimer = 0.0f;
    private float _skeletonMaxHp;
    private float _skeletonCurHp;

    private float _hpBarVisibleTimer = 0.0f;
    private float _hpBarVisibleDuration = 3.0f;
    private bool _hpBarVisible = false;

    private bool _isFadingOut = false;
    private float _fadeSpeed = 5.0f;

    private bool _isCollidingWithPlayer = false;

    public float SkeletonCurHp
    {
        get { return _skeletonCurHp; }
    }

    private void OnEnable()
    {
        _skeletonCurHp = _skeletonMaxHp;

        if (_skeletonHpBarSlider != null)
        {
            _skeletonHpBarSlider.gameObject.SetActive(false);
            SetHpBarAlpha(0.5f);
        }

        if (_skeletonCollider != null)
            _skeletonCollider.enabled = true;

        _hpBarVisible = false;
        _hpBarVisibleTimer = 0.0f;
        _isFadingOut = false;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _skeletonHpBarPrefab = Resources.Load<GameObject>("Prefabs/Monsters/MonsterHpBar");
        _skeletonHpBarOffset = new Vector3(0, 2.5f, 1);
    }

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _playerHpExp = GameObject.FindWithTag("Player").GetComponent<PlayerHpExp>();
        _monsterData = MonsterDataManager.Instance.GetMonsterData(201);
        _skeletonMaxHp = _monsterData.Hp;
        _skeletonCurHp = _skeletonMaxHp;

        GameObject skeletonhpBarPanel = GameObject.Find("MonsterHpBarPanel");
        GameObject skeletonhpBar = Instantiate(_skeletonHpBarPrefab, skeletonhpBarPanel.transform);
        _skeletonHpBarSlider = skeletonhpBar.GetComponent<Slider>();

        Transform imageRoot = _skeletonHpBarSlider.transform.Find("MonsterHpBarBG");
        _hpBGImage = imageRoot.GetComponent<Image>();
        _hpFillImage = imageRoot.Find("MonsterHp")?.GetComponent<Image>();
        _hpBLImage = imageRoot.Find("MonsterHpBarBL")?.GetComponent<Image>();


        _skeletonHpBarSlider.gameObject.SetActive(false);
        SetHpBarAlpha(0.5f);

        _skeletonCollider = GetComponent<Collider>();
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        bool isInDamage = stateInfo.IsName("GetDamage");
        bool isInDead = stateInfo.IsName("Dead");

        if (_skeletonHpBarSlider != null && _skeletonHpBarSlider.gameObject.activeSelf)
        {
            Vector3 worldPosition = transform.position + _skeletonHpBarOffset;
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

            if (screenPosition.z > 0)
            {
                _skeletonHpBarSlider.transform.position = screenPosition;
                _skeletonHpBarSlider.value = _skeletonCurHp / _skeletonMaxHp;
            }
            else
            {
                _skeletonHpBarSlider.gameObject.SetActive(false);
            }
        }

        if (_hpBarVisible)
        {
            _hpBarVisibleTimer += Time.deltaTime;
            if (_hpBarVisibleTimer >= _hpBarVisibleDuration)
            {
                _hpBarVisible = false;
                _isFadingOut = true;
            }
        }

        if (_isFadingOut)
        {
            float newAlpha = Mathf.Lerp(_hpFillImage.color.a, 0, Time.deltaTime * _fadeSpeed);
            SetHpBarAlpha(newAlpha);

            if (newAlpha <= 0.05f)
            {
                _skeletonHpBarSlider.gameObject.SetActive(false);
                _isFadingOut = false;
            }
        }

        if (isInDamage || isInDead) return;

        if (_player != null && _skeletonCurHp > 0)
        {
            Vector3 direction = _player.position - transform.position;
            direction.y = 0;

            if (direction.sqrMagnitude > 0)
            {
                transform.position += direction.normalized * _monsterData.MoveSpeed * Time.deltaTime;

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _skeletonRotateSpeed);
            }
        }

        if (_isCollidingWithPlayer && _skeletonCurHp > 0)
        {
            _attackTimer += Time.deltaTime;
            if(_attackTimer >= _monsterData.AttackInterval)
            {
                _attackTimer -= _monsterData.AttackInterval;
                SkeletonAttackPlayer();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isCollidingWithPlayer = true;
            _attackTimer = _monsterData.AttackInterval;
        }

        if (other.CompareTag("Weapon"))
        {
            if (_skeletonHpBarSlider != null)
            {
                _skeletonHpBarSlider.gameObject.SetActive(true);
                SetHpBarAlpha(1.0f);

                _hpBarVisible = true;
                _hpBarVisibleTimer = 0.0f;
                _isFadingOut = false;
            }

            GetSkeletonDamage(other.GetComponent<Weapon>().WeaponAttackPower);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isCollidingWithPlayer = false;
            _attackTimer = 0.0f;
        }
    }

    private void SkeletonAttackPlayer()
    {
        if (Player.Instance != null)
        {
            _playerHpExp.TakeDamage(_monsterData.AttackPower);
        }
    }

    private void GetSkeletonDamage(float attackPower)
    {
        if (_skeletonCurHp <= 0) return;

        _skeletonCurHp -= attackPower;

        if (_skeletonCurHp <= 0)
        {
            _skeletonCurHp= 0;
            _skeletonCollider.enabled = false;

            if (_animator != null)
                _animator.SetTrigger("Die");

            _skeletonHpBarSlider.gameObject.SetActive(false);
            _hpBarVisible = false;
        }
        else
        {
            if (_animator != null)
            {
                _animator.SetTrigger("Hit");
            }
        }
    }

    public void HandleDeath()
    {
        gameObject.SetActive(false);

        _playerHpExp.GetComponent<PlayerHpExp>().GetExp(_monsterData.Exp);
    }

    private void SetHpBarAlpha(float alpha)
    {
        if (_hpBLImage != null)
        {
            Color hpBLImageColor = _hpBLImage.color;
            hpBLImageColor.a = alpha;
            _hpBLImage.color = hpBLImageColor;
        }

        if (_hpFillImage != null)
        {
            Color hpFillImageColor = _hpFillImage.color;
            hpFillImageColor.a = alpha;
            _hpFillImage.color = hpFillImageColor;
        }

        if (_hpBGImage != null)
        {
            Color hpBGImageColor = _hpBGImage.color;
            hpBGImageColor.a = alpha;
            _hpBGImage.color = hpBGImageColor;
        }
    }
}
