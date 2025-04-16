using UnityEngine;
using UnityEngine.UI;

public class Skeleton : MonoBehaviour
{
    private Transform _player;
    private GameObject _skeletonHpBarPrefab;
    private Slider _skeletonHpBarSlider;

    private Vector3 _skeletonHpBarOffset;
    private MonsterData _monsterData;

    private float _skeletonRotateSpeed = 5.0f;
    private float _attackTimer = 0.0f;
    private float _skeletonMaxHp;
    private float _skeletonCurHp;

    private bool _isCollidingWithPlayer = false;

    private void OnEnable()
    {
        _skeletonCurHp = _skeletonMaxHp;

        if (_skeletonHpBarSlider != null)
            _skeletonHpBarSlider.gameObject.SetActive(true);
    }

    private void Awake()
    {
        _skeletonHpBarPrefab = Resources.Load<GameObject>("Prefabs/Monsters/MonsterHpBar");
        _skeletonHpBarOffset = new Vector3(0, 2.5f, 1);
    }

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _monsterData = MonsterDataManager.Instance.GetMonsterData(201);
        _skeletonMaxHp = _monsterData.Hp;
        _skeletonCurHp = _skeletonMaxHp;

        GameObject skeletonhpBarPanel = GameObject.Find("MonsterHpBarPanel");
        GameObject skeletonhpBar = Instantiate(_skeletonHpBarPrefab, skeletonhpBarPanel.transform);
        _skeletonHpBarSlider = skeletonhpBar.GetComponent<Slider>();

        _skeletonHpBarSlider.gameObject.SetActive(true);
    }

    void Update()
    {
        if (_player != null)
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

        if (_isCollidingWithPlayer)
        {
            _attackTimer += Time.deltaTime;
            if(_attackTimer >= _monsterData.AttackInterval)
            {
                SkeletonAttackPlayer();
                _attackTimer = 0.0f;
            }
        }

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isCollidingWithPlayer = true;
            _attackTimer = _monsterData.AttackInterval;
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
            Player.Instance.TakeDamage(_monsterData.AttackPower);
        }
    }

    public void GetSkeletonDamage(float attackPower)
    {
        _skeletonCurHp -= attackPower;

        if (_skeletonCurHp < 0)
        {
            gameObject.SetActive(false);

            _player.GetComponent<Player>().GetExp(_monsterData.Exp);

            if (_skeletonHpBarSlider != null)
                _skeletonHpBarSlider.gameObject.SetActive(false);
        }
    }
}
