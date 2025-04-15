using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Animator anim;
    private Slider healthSlider;
    private float maxHealth = 100f;
    private float currentHealth;
    
    public enum State
    {
        Idle,
        Walking,
        Running,
        Dead
    }

    private static Player _instance;
    public static Player Instance
    {
        get { return _instance; }
    }

    private State currentState = State.Idle;

    private float _playerMoveSpeed = 6.0f;
    private float _playerRotateSpeed = 12.0f;

    /*private float walkSpeed = 6.0f;
    private float runSpeed = 12.0f;
    private float rotateSpeed = 12.0f;*/
    //Animator animator;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        healthSlider = GameObject.Find("PlayerHpBar").GetComponent<Slider>();
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (inputDir.sqrMagnitude > 0)
        {
            anim.SetFloat("Run", inputDir.sqrMagnitude);
            transform.Translate(inputDir.normalized * _playerMoveSpeed * Time.deltaTime, Space.World); //position += inputDir * playerMoveSpeed * Time.deltaTime;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(inputDir), Time.deltaTime * _playerRotateSpeed);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10f);
        }
        anim.SetFloat("Idle", inputDir.sqrMagnitude);
        // 상태 전이
        /*  if (inputDir.sqrMagnitude == 0)
          {
              ChangeState(State.Idle);
              animator.SetTrigger("Walk");
          }
          else if (Input.GetKey(KeyCode.LeftShift))
          {
              ChangeState(State.Running);
              animator.SetTrigger("Run");
          }
          else
          {
              ChangeState(State.Walking);
              animator.SetTrigger("Walk");

          }*/

        //Move(inputDir);
    }
    /*private void Move(Vector3 dir)
    {
        if (dir.sqrMagnitude == 0) return;

        float moveSpeed = (currentState == State.Running) ? runSpeed : walkSpeed;
        dir.Normalize();


        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotateSpeed);
    }

    void ChangeState(State newState)
    {
        if (currentState == newState) return;

        currentState = newState;
    }*/
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
            healthSlider.value = currentHealth;
    }
}

