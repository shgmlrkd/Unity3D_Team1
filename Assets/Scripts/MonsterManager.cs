using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private GameObject SkeletonPrefab;
    private List<GameObject> _skeletonPool = new List<GameObject>();

    private int poolSize = 5;

    void Start()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Monsters/MeleeMonster");
        SkeletonPrefab = prefab;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(SkeletonPrefab);
            obj.SetActive(false);
            _skeletonPool.Add(obj);
        }

        SpawnMonsters();
    }

    void Update()
    {
        
    }

    void SpawnMonsters()
    {
        Camera cam = Camera.main;
        float spawnDistance = 5f;

        List<Vector3> spawnPositions = new List<Vector3>();

        spawnPositions.Add(GetRandomPositionOutside(cam, new Vector2(0.1f, 2.2f), new Vector2(0.9f, 2.2f)));
        
        spawnPositions.Add(GetRandomPositionOutside(cam, new Vector2(0.1f, -0.8f), new Vector2(0.9f, -0.8f)));
        
        spawnPositions.Add(GetRandomPositionOutside(cam, new Vector2(-0.2f, 0.1f), new Vector2(-0.2f, 0.9f)));

        spawnPositions.Add(GetRandomPositionOutside(cam, new Vector2(1.2f, 0.1f), new Vector2(1.2f, 0.9f)));

        int randomSide = Random.Range(0, 4);
        spawnPositions.Add(spawnPositions[randomSide]);

        for (int i = 0; i < _skeletonPool.Count && i < spawnPositions.Count; i++)
        {
            GameObject obj = _skeletonPool[i];
            obj.transform.position = spawnPositions[i];
            obj.SetActive(true);
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
