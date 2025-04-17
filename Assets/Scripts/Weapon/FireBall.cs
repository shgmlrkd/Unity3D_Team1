using System.Collections;
using UnityEngine;

public class FireBall : ThrowWeapon
{
    private GameObject[] _fireBallChildren;
    private SphereCollider[] _fireBallColliders;
    private ParticleSystem _fireBallExplosionParticle;

    private bool _isStartPlayParticle = false;

    private enum FireBallChild
    {
        FireBallSphere, FireBallExplosionVFX
    }

    private void Awake()
    {
        int count = transform.childCount;
        _fireBallChildren = new GameObject[count];
        _fireBallColliders = new SphereCollider[count];

        for (int i = 0; i < count; i++)
        {
            _fireBallChildren[i] = transform.GetChild(i).gameObject;
            _fireBallColliders[i] = transform.GetChild(i).gameObject.GetComponent<SphereCollider>();
        }
    }

    private void Start()
    {
        _fireBallExplosionParticle = _fireBallChildren[(int)FireBallChild.FireBallExplosionVFX].GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        _isStartPlayParticle = false;
        _fireBallChildren[(int)FireBallChild.FireBallSphere].SetActive(true);
        _fireBallChildren[(int)FireBallChild.FireBallExplosionVFX].SetActive(false);
    }

    private new void Update()
    {
        if (_isStartPlayParticle && !_fireBallExplosionParticle.isPlaying)
        {
            gameObject.SetActive(false);
        }

        if (_fireBallChildren[(int)FireBallChild.FireBallSphere].activeSelf)
        {
            _timer += Time.deltaTime;

            if (_timer >= _weaponLifeTimer)
            {
                _timer -= _weaponLifeTimer;
                _fireBallChildren[(int)FireBallChild.FireBallSphere].SetActive(false);
            }
        }

        if (_isStartPlayParticle) return;

        transform.Translate(Vector3.forward * _weaponSpeed * Time.deltaTime);
    }

    public void Fire(Vector3 pos, Vector3 dir, WeaponData data)
    {
        gameObject.SetActive(true);

        transform.position = pos;
        _direction = dir.normalized;
        _weaponSpeed = data.AttackSpeed;
        _weaponAttackPower = data.AttackPower;
        _weaponLifeTimer = data.LifeTime;
        _fireBallColliders[(int)FireBallChild.FireBallExplosionVFX].radius = data.AttackRange;
        _direction.y = 0.0f;

        transform.rotation = Quaternion.LookRotation(_direction);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag("Monster"))
        {
            _fireBallChildren[(int)FireBallChild.FireBallSphere].SetActive(false);
            print("펑!");
            _fireBallChildren[(int)FireBallChild.FireBallExplosionVFX].SetActive(true);
            print("터지는 효과");
            _isStartPlayParticle = true;
            StartCoroutine(ExplosionColliderOffTimer());
        }
    }

    private IEnumerator ExplosionColliderOffTimer()
    {
        yield return new WaitForSeconds(0.2f);

        _fireBallColliders[(int)FireBallChild.FireBallExplosionVFX].enabled = false;
    }
}