using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class BulletSkill : MonoBehaviour
{
    private WaitForSeconds _fireInterval;

    private WeaponData _weaponData;

    private int _bulletIndexKey = 300;
    private int _level = 1;

    private void Awake()
    {
        _weaponData = WeaponDataManager.Instance.GetWeaponData(_bulletIndexKey + _level);
        _fireInterval = new WaitForSeconds(_weaponData.AttackInterval);
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
