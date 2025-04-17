using System.Collections;
using UnityEngine;

public class ArrowSkill : Skill
{
    private int _arrowIndexKey = 306;

    private void Awake()
    {
        _weaponData = WeaponDataManager.Instance.GetWeaponData(_arrowIndexKey);
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
        BulletManager.Instance.Fire(transform.position, dir, _weaponData);
    }
}
