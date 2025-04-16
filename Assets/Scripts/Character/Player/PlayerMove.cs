using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Animator _anim;
    private float _playerMoveSpeed;
    private float _playerRotateSpeed = 12.0f;
    private bool _isRunning = false;
    void Start()
    {
        _anim = GetComponent<Animator>();
        _playerMoveSpeed = PlayerDataManager.Instance.GetPlayerData(1).Speed;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (inputDir.sqrMagnitude > 0)
        {
            _isRunning = true;
            transform.Translate(inputDir.normalized * _playerMoveSpeed * Time.deltaTime, Space.World);

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(inputDir), Time.deltaTime * _playerRotateSpeed);
        }
        else
        {
            _isRunning = false;
        }

        _anim.SetBool("IsRunning", _isRunning);
    }

    public void PlayerSpeedUp(int level)
    {
        _playerMoveSpeed = PlayerDataManager.Instance.GetPlayerData(level).Speed;
    }
}
