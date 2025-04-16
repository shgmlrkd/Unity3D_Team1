using System.Collections.Generic;
using UnityEngine;

public struct MonsterData
{
    public int key;
    public string name;
    public string type;
    public int hp;
    public float moveSpeed;
    public int attackPower;
    public float attackInterval;
    public int distance;
    public int lifeTime;
    public int playerLevel;
    public float spawnInterval;
    public int exp;
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private Dictionary<int, MonsterData> _monsterDatas = new Dictionary<int, MonsterData>();
    private void Awake()
    {
        Instance = this;

        LoadMonsterData();
    }

    public MonsterData GetMonsterData(int key)
    {
        return _monsterDatas[key];
    }

    private void LoadMonsterData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("GameDataFolder/MonsterTable");

        string[] rowData = textAsset.text.Split("\r\n");

        for(int i = 1; i < rowData.Length; i++)
        {
            string[] colData = rowData[i].Split(",");

            if (colData.Length <= 1)
                return;

            MonsterData data;
            data.key = int.Parse(colData[0]);
            data.name = colData[1];
            data.type = colData[2];
            data.hp = int.Parse(colData[3]);
            data.moveSpeed = float.Parse(colData[4]);
            data.attackPower = int.Parse(colData[5]);
            data.attackInterval = float.Parse(colData[6]);
            data.distance = int.Parse(colData[7]);
            data.lifeTime = int.Parse(colData[8]);
            data.playerLevel = int.Parse(colData[9]);
            data.spawnInterval = float.Parse(colData[10]);
            data.exp = int.Parse(colData[11]);

            _monsterDatas.Add(data.key, data);
        }
    }
}
