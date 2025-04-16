using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Animator _anim;
    private float _playerMoveSpeed = 100.0f;
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
        _anim.SetBool("IsRunning", _isRunning);
    }
    private void Move()
    {
        Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (inputDir.sqrMagnitude > 0)
        {
            _isRunning = true;
            transform.Translate(inputDir.normalized * 6* Time.deltaTime, Space.World); //position += inputDir * playerMoveSpeed * Time.deltaTime;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(inputDir), Time.deltaTime * _playerRotateSpeed);
        }
        else
        {
            _isRunning = false;
        }
    }
}
