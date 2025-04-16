using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform _target;

    private Vector3 targetPos;
    private Vector3 destPos;

    private float _distance = 9.89f;
    private float _height = 16.8f;
    private float _moveDamping = 10.0f;

    private void Awake()
    {
        
    }

    private void Start()
    {
        _target = GameManager.Instance.Player.transform;

        targetPos = _target.position;
        destPos = targetPos + Vector3.back * _distance + Vector3.up * _height;
        transform.position = destPos;
    }

    private void LateUpdate()
    {
        if (!_target) return;

        targetPos = _target.position;
        destPos = targetPos + Vector3.back * _distance + Vector3.up * _height;

        transform.position = Vector3.Lerp(transform.position, destPos, _moveDamping * Time.deltaTime);
    }
}
