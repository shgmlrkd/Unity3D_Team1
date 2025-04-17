using UnityEngine;

public class Kunai : ThrowWeapon
{
    private int _pierce = 0;

    private void OnEnable()
    {
        _timer = 0.0f;
        _pierce = 0;
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
        _direction = dir;
        _weaponSpeed = data.AttackSpeed;
        _weaponAttackPower = data.AttackPower;
        _weaponLifeTimer = data.LifeTime;
        _weaponPierce = data.Pierce;

        transform.rotation = Quaternion.LookRotation(_direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if(other.CompareTag("Monster"))
        {
            if( _pierce == _weaponPierce)
            {
                gameObject.SetActive(false);
            }

            _pierce++;
        }
    }
}
