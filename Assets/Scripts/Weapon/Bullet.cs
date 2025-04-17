using System.Collections.Generic;
using UnityEngine;

public class Bullet : Weapon
{
    private void OnEnable()
    {
        _timer = 0.0f;
    }
    
    void Update()
    {
        LifeTimer();

        transform.Translate(Vector3.forward * _weaponSpeed * Time.deltaTime);
    }

    public void Fire(Vector3 pos, Vector3 dir, WeaponData data)
    {
        gameObject.SetActive(true);

        transform.position = pos;
        _direction = dir.normalized;
        _weaponSpeed = data.AttackSpeed;
        _weaponAttackPower = data.AttackPower;
        _weaponLifeTimer = data.LifeTime;

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