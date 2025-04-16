using UnityEngine;

public class Skeleton : MonoBehaviour
{
    private Transform _player;
    private MonsterData _monsterData;

    private float skeletonRotateSpeed = 5.0f;
    private float _attackTimer = 0.0f;
    private bool _isCollidingWithPlayer = false;

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _monsterData = MonsterDataManager.Instance.GetMonsterData(201);
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
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * skeletonRotateSpeed);
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

    private void GetSkeletonDamage(float attackPower)
    {
        _monsterData.Hp -= attackPower;

        if (_monsterData.Hp < 0)
        {
            gameObject.SetActive(false);
        }
    }
}
