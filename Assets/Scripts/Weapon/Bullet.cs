using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private List<GameObject> _monsters; 

    private Vector3 _direction;

    private float _bulletSpeed;
    private float _bulletPower;
    private float _bulletLifeTimer;
    private float _timer = 0.0f;

    private void OnEnable()
    {
        _timer = 0.0f;
    }

    void Start()
    {
        _monsters = MonsterManager.Instance.SkeletonPool;
    }
    
    void Update()
    {
        if (gameObject.activeSelf)
        {
            _timer += Time.deltaTime;

            if (_timer >= _bulletLifeTimer)
            {
                _timer -= _bulletLifeTimer;
                gameObject.SetActive(false);
            }
        }

        transform.Translate(Vector3.forward * _bulletSpeed * Time.deltaTime);
    }

    public void Fire(Vector3 pos, Vector3 dir, WeaponData data)
    {
        gameObject.SetActive(true);

        transform.position = pos;
        _direction = dir.normalized;
        _bulletSpeed = data.AttackSpeed;
        _bulletPower = data.AttackPower;
        _bulletLifeTimer = data.LifeTime;

        transform.rotation = Quaternion.LookRotation(_direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster"))
        {
            other.GetComponent<Skeleton>().GetSkeletonDamage(_bulletPower);
            gameObject.SetActive(false);
        }
    }
}