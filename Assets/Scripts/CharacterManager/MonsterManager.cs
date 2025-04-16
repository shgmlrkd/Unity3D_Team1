using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private GameObject SkeletonPrefab;
    private List<GameObject> _skeletonPool;

    private float _timer = 0.0f;
    private float _spawnInterval = 1.5f;

    private int _poolSize = 5;

    private int _groundLayer;

    private void Awake()
    {
        _groundLayer = LayerMask.GetMask("Ground");
        _skeletonPool = new List<GameObject>();
    }

    void Start()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Monsters/MeleeMonster");
        SkeletonPrefab = prefab;

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject obj = Instantiate(SkeletonPrefab);
            obj.SetActive(false);
            _skeletonPool.Add(obj);
        }

        SpawnMonsters();
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _spawnInterval)
        {
            _timer -= _spawnInterval;
            SpawnMonsters();
        }
    }

    void SpawnMonsters()
    {
        int randomSide = Random.Range(0, 4);
        Vector3 screenPos = GetRandomScreenPositionBySide(randomSide);
        Vector3? groundPos = GetGroundPositionFromScreen(screenPos);

        if (groundPos == null) return;

        for (int i = 0; i < _skeletonPool.Count; i++)
        {
            if (!_skeletonPool[i].activeSelf)
            {
                _skeletonPool[i].transform.position = groundPos.Value;
                _skeletonPool[i].SetActive(true);
                return;
            }
        }
    }

    Vector3 GetRandomScreenPositionBySide(int side)
    {
        switch (side)
        {
            case 0: return new Vector3(Random.Range(0, Screen.width), Screen.height, 0); //위
            case 1: return new Vector3(Random.Range(0, Screen.width), 0, 0); // 아래
            case 2: return new Vector3(0, Random.Range(0, Screen.height), 0); // 왼쪽
            case 3: return new Vector3(Screen.width, Random.Range(0, Screen.height), 0); // 오른쪽
            default: return Vector3.zero;
        }
    }

    Vector3? GetGroundPositionFromScreen(Vector3 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000.0f, _groundLayer))
        {
            return hit.point;
        }
        return null;
    }
}
