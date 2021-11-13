using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//�^�X�N����������O�ɓ����^�X�N���J��Ԃ��ƈӐ}���Ȃ����Ԃ��N����̂ŕK���L�����m�F����

public class GaussBullet : MonoBehaviour
{
    Rigidbody _rb;
    Vector3 velocity;
    Collider _collider;
    [SerializeField] float cannonPower = 5.0f;
    [SerializeField] float effectiveRange = 300.0f;
    [SerializeField] float dissolveTime = 1.0f;
    [SerializeField] float _maxReflectAngle = 160.0f;

    ObjectPool _objectPool;
    [Header("�I�u�W�F�N�g�v�[���ɐݒ肷�����")]
    [SerializeField] GameObject _explodeEffect;
    [SerializeField] int _startExplodeEffectNum;
    [SerializeField] GameObject _hitWallEffect;
    [SerializeField] int _startHitWallEffectNum;

    Vector3 startPosi = Vector3.zero;
    Quaternion forwardDire;

    bool willDeactive = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _objectPool = GetComponent<ObjectPool>();
        Reset();
    }

    // Start is called before the first frame update
    void Start()
    {
        _objectPool.CreatePool(_explodeEffect, _startExplodeEffectNum);
        _objectPool.CreatePool(_hitWallEffect, _startHitWallEffectNum);
    }

    private void OnEnable()
    {
        forwardDire = transform.rotation;
        if (_rb == null) _rb = GetComponent<Rigidbody>();
        Reset();
        MoveFoward(transform.forward, cannonPower);
    }

    void FixedUpdate()
    {
        velocity = _rb.velocity;
        FadeGaussBullet(startPosi, effectiveRange);
    }

    private void Reset()
    {
        willDeactive = false;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _collider.enabled = true;
    }

    /// <summary>
    /// ���ˈʒu����̋����ŏ�����悤�ɂ��Ă���
    /// </summary>
    /// <param name="startBulletPosi"></param>
    /// <param name="maxEffectiveRange"></param>
    void FadeGaussBullet(Vector3 startBulletPosi, float maxEffectiveRange)
    {
        if (!willDeactive && Vector3.Distance(transform.position, startBulletPosi) > maxEffectiveRange)
        {
            willDeactive = true;
            DOVirtual.DelayedCall(dissolveTime, () =>
            {
                gameObject.SetActive(false);
            }, false);
        }
    }

    public void MoveFoward(Vector3 moveVector, float speed)
    {
        _rb.velocity = moveVector * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�G�t�F�N�g�̍Đ�
        var collisionEnemyDeath = collision.gameObject.GetComponent<EnemyDeath>();
        if (collision.gameObject.CompareTag("Ground"))
        {
            _objectPool.GetObject(_hitWallEffect, transform.position, Quaternion.identity);
        }
        else
        {
            _objectPool.GetObject(_explodeEffect, transform.position, Quaternion.identity); //�����G�t�F�N�g����
        }
        
        //�q�b�g�������̂��G���ǂ�������
        if (collisionEnemyDeath != null)
        {
            collisionEnemyDeath.OnDead();
            if (collisionEnemyDeath.Penetratable)
            {
                transform.rotation = forwardDire; //�ђʂ����邽�ߊp�x��ς����ɂ��̂܂܂̐����Ői�܂���
                MoveFoward(velocity, 1.0f);
                return; //�ђʂ���Ȃ�Δ��˂̌v�Z�������s���K�v���Ȃ��̂ł�����return
            }
        }

        Vector3 reflect = Vector3.Reflect(velocity, collision.contacts[0].normal);
        float reflectAngle = Vector3.Angle(velocity, reflect);

        if (reflectAngle < _maxReflectAngle)
        {
            forwardDire = transform.rotation;
            reflect = reflect.normalized;
            MoveFoward(reflect, cannonPower);
        }
        else
        {
            _collider.enabled = false;
            MoveFoward(Vector3.zero, 0);
            //dissolveTime�ϐ��̎��Ԍo�ߌ�I�u�W�F�N�g���A�N�e�B�u�ɂ���
            DOVirtual.DelayedCall(dissolveTime, () =>
            {
                gameObject.SetActive(false);
            }, false);
        }
    }
}
