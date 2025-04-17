using System.Collections;
using UnityEngine;

public class FireBallSkill : Skill
{
    private int _bulletIndexKey = 321;

    private void Awake()
    {
        _weaponData = WeaponDataManager.Instance.GetWeaponData(_bulletIndexKey);
        InitInterval(_weaponData);
    }

    void Start()
    {
        StartCoroutine(FireLoop());
    }

    private IEnumerator FireLoop()
    {
        while (true)
        {
            Fire();

            yield return _fireInterval;
        }
    }

    private void Fire()
    {
        GameObject target = MonsterManager.Instance.GetClosestMonster(transform.position);

        if (target == null)
            return;

        Vector3 dir = target.transform.position - transform.position;

        WeaponManager.Instance.FireBallFire(transform.position, dir, _weaponData);
    }
}