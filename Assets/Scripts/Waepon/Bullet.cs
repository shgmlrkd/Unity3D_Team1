using UnityEngine;

public class Bullet : MonoBehaviour
{
    private WeaponData _weaponData;
    private Transform _player;
    void Start()
    {
        _player = GetComponent<Transform>();
        _weaponData = WeaponDataManager.Instance.GetWeaponData(301);
    }
    
    void Update()
    {
        
    }
}