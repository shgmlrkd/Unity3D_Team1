using UnityEngine;

public class Skeleton : MonoBehaviour
{
    private Transform _player;
    private MonsterData _monsterData;

    private float skeletonRotateSpeed = 5.0f;
    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _monsterData = MonsterDataManager.Instance.GetMonsterData(201);
    }

    void Update()
    {
        if (_player != null)
        {
            Vector3 direction = _player.position - transform.position;
            direction.y = 0;

            if (direction.sqrMagnitude > 0)
            {
                transform.position += direction.normalized * _monsterData.moveSpeed * Time.deltaTime;

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * skeletonRotateSpeed);
            }
        }
    }
}
