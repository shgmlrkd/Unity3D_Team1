using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected Vector3 _direction;

    protected float _weaponSpeed;
    protected float _weaponAttackPower;
    public float WeaponAttackPower
    {
        get { return _weaponAttackPower; }
    }
    protected float _weaponLifeTimer;
    protected float _timer = 0.0f;

    protected void LifeTimer()
    {
        if (gameObject.activeSelf)
        {
            _timer += Time.deltaTime;

            if (_timer >= _weaponLifeTimer)
            {
                _timer -= _weaponLifeTimer;
                gameObject.SetActive(false);
            }
        }
    }
}
