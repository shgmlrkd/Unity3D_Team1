using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get { return _instance; }
    }

    private GameObject _player;
    public GameObject Player
    { get { return _player; } }

    [SerializeField]
    private int _bulletPoolSize; 
    [SerializeField]
    private int _kunaiPoolSize;

    void Awake()
    {
        _instance = this;
        WeaponManager.Instance.CreateWeapons(_bulletPoolSize, "Bullet");
        WeaponManager.Instance.CreateWeapons(_kunaiPoolSize, "Kunai");
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Player");

        _player = Instantiate(prefab);
    }
}