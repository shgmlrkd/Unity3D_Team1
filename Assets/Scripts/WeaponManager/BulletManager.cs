using UnityEngine;

public class BulletManager : Singleton<BulletManager>
{
    public void CreateBullets(int poolSize)
    {
        GameObject bulletPrefab = Resources.Load<GameObject>("Prefabs/Weapons/Bullet");
        PoolingManager.Instance.Add("Bullet", poolSize, bulletPrefab, transform);
    }

    public void Fire(Vector3 pos, Vector3 dir, WeaponData data)
    {
        GameObject bullet = PoolingManager.Instance.Pop("Bullet");
        bullet.GetComponent<Bullet>().Fire(pos, dir, data);
    }
}