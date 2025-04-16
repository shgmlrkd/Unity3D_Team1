using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    private Vector3 _aimOffset;
    [SerializeField]
    private float _distance = 30.0f;
    [SerializeField]
    private float _height = 30.0f;
    [SerializeField]
    private float _moveDamping = 10.0f;

    private Transform _target;

    private void Awake()
    {
        
    }

    private void Start()
    {
        _target = GameManager.Instance.Player.transform;
    }

    private void LateUpdate()
    {
        if (!_target) return;

        Vector3 targetPos = _target.position + _aimOffset;

        Vector3 destPos = targetPos + Vector3.back * _distance + Vector3.up * _height;

        transform.position = Vector3.Lerp(transform.position, destPos, _moveDamping * Time.deltaTime);
    }
}
