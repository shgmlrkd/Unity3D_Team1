using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private Camera Cam;
    private GameObject SkeletonPrefab;
    private List<GameObject> _skeletonPool;

    private float _timer = 0.0f;
    private float _spawnInterval = 1.5f;

    private int _poolSize = 5;

    private void Awake()
    {
        Cam = Camera.main;
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
        Vector3 spawnPos = GetRandomPositionBySide(randomSide);

        for (int i = 0; i < _skeletonPool.Count; i++)
        {
            if (!_skeletonPool[i].activeSelf)
            {
                _skeletonPool[i].transform.position = spawnPos;
                _skeletonPool[i].SetActive(true);
                return;
            }
        }
    }

    Vector3 GetRandomPositionBySide(int side)
    {
        switch (side)
        {
            case 0: return GetRandomPositionOutside(Cam, new Vector2(0.1f, 2.2f), new Vector2(0.9f, 2.2f)); // 위
            case 1: return GetRandomPositionOutside(Cam, new Vector2(0.1f, -0.8f), new Vector2(0.9f, -0.8f)); // 아래
            case 2: return GetRandomPositionOutside(Cam, new Vector2(-0.2f, 0.1f), new Vector2(-0.2f, 0.9f)); // 왼쪽
            case 3: return GetRandomPositionOutside(Cam, new Vector2(1.2f, 0.1f), new Vector2(1.2f, 0.9f)); // 오른쪽
            default: return Vector3.zero;
        }
    }

    Vector3 GetRandomPositionOutside(Camera cam, Vector2 minViewport, Vector2 maxViewport)
    {
        float x = Random.Range(minViewport.x, maxViewport.x);
        float y = Random.Range(minViewport.y, maxViewport.y);
        Vector3 viewportPos = new Vector3(x, y, cam.nearClipPlane + 20f);

        Vector3 worldPos = cam.ViewportToWorldPoint(viewportPos);
        worldPos.y = 0;
        return worldPos;
    }
}
