using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private List<GameObject> _monsters; 

    private Vector3 _direction;

    private float _bulletSpeed;
    private float _bulletPower;

    void Start()
    {
        _monsters = MonsterManager.Instance.SkeletonPool;
    }
    
    void Update()
    {
        transform.Translate(Vector3.forward * _bulletSpeed * Time.deltaTime);
    }

    public void Fire(Vector3 pos, Vector3 dir, WeaponData data)
    {
        gameObject.SetActive(true);

        transform.position = pos;
        _direction = dir.normalized;
        _bulletSpeed = data.AttackSpeed;
        _bulletPower = data.AttackPower;

        transform.rotation = Quaternion.LookRotation(_direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster"))
        {
            gameObject.SetActive(false);
        }
    }
}