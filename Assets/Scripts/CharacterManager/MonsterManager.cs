using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private static MonsterManager _instance;
    public static MonsterManager Instance
    { 
        get { return _instance; }
    }

    // GameObject�� ����(Skeleton, Lich) �������� ���� �����
    // List<GameObject>���� ���� ���������� ������ �ֵ��� ��� �������
    private GameObject SkeletonPrefab;
    private List<GameObject> _skeletonPool;
    public List<GameObject> SkeletonPool 
    { 
        get { return _skeletonPool; }
    }

    private readonly int _spawnPosOffset = 50;

    private float _timer = 0.0f;
    private float _spawnInterval = 1.5f;

    private int _poolSize = 100;

    private int _groundLayer;

    private void Awake()
    {
        _instance = this;

        _groundLayer = LayerMask.GetMask("Ground");
        _skeletonPool = new List<GameObject>();
    }

    void Start()
    {
        // ���⼭ prefab�� MeleeMonster�� RangeMosnter�� ��� for������ Ǯ�� ������ŭ �̰�
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Monsters/MeleeMonster");
        SkeletonPrefab = prefab;

        // ���⿡�� List<GameObject>�� add�ϴ� ������ �׷��� ��� ���Ͱ� �� List �ȿ� ������
        // ���� �������� �Ҷ� �������� ����
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
        Vector3 groundPos = GetGroundPositionFromScreen(screenPos);

        if (groundPos == Vector3.zero) return;

        for (int i = 0; i < _skeletonPool.Count; i++)
        {
            if (!_skeletonPool[i].activeSelf)
            {
                _skeletonPool[i].transform.position = groundPos;
                _skeletonPool[i].SetActive(true);
                return;
            }
        }
    }

    Vector3 GetRandomScreenPositionBySide(int side)
    {
        switch (side)
        {
            case 0: return new Vector3(Random.Range(0, Screen.width), Screen.height + _spawnPosOffset, 0); //��
            case 1: return new Vector3(Random.Range(0, Screen.width), -(_spawnPosOffset), 0); // �Ʒ�
            case 2: return new Vector3(-(_spawnPosOffset), Random.Range(0, Screen.height), 0); // ����
            case 3: return new Vector3(Screen.width + _spawnPosOffset, Random.Range(0, Screen.height), 0); // ������
            default: return Vector3.zero;
        }
    }

    Vector3 GetGroundPositionFromScreen(Vector3 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000.0f, _groundLayer))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    public GameObject GetClosestMonster(Vector3 pos)
    {
        GameObject selectEnemy = null;
        float minDistance = float.MaxValue;

        foreach (GameObject monster in _skeletonPool)
        {
            if (!monster.activeSelf || monster.GetComponent<Skeleton>().SkeletonCurHp <= 0) continue;

            float distance = Vector3.Distance(pos, monster.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                selectEnemy = monster;
            }

        }
        return selectEnemy;
    }
}
