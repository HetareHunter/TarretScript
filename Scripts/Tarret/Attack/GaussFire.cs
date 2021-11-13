using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GaussFire : MonoBehaviour
{
    ObjectPool _objectPool;
    [Header("�I�u�W�F�N�g�v�[���ɐݒ肷�����")]
    [SerializeField] GameObject _objectPoolObj;
    /// <summary>�r�[���{�̂̃G�t�F�N�g </summary>
    [SerializeField] GameObject _razerEffect;
    [SerializeField] int _razerEffectNum = 1;

    [SerializeField] GameObject _bulletEffect;
    [SerializeField] int _bulletEffectNum = 3;
    /// <summary>�e�̐����ʒu </summary>
    [SerializeField] GameObject _gaussEffectInsPosi;

    bool _lockOn = false;
    Vector3 _target;

    public Vector3 SetTarget
    {
        set
        {
            _target = value;
            _lockOn = true;
        }
    }

    public bool TargetUnlock
    {
        set
        {
            _lockOn = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _objectPool = _objectPoolObj.GetComponent<ObjectPool>();
        _objectPool.CreatePool(_razerEffect, _razerEffectNum);
        _objectPool.CreatePool(_bulletEffect, _bulletEffectNum);
    }

    /// <summary>
    /// ���[�U�[�̃��C�������̃X�N���v�g�A���݂��Ă�����̂��ړ����āA�A���X�ɏ����Ă����悤�ɂ��Ă���
    /// </summary>
    public void InstanceFireEffect()
    {
        _objectPool.GetObject(_razerEffect, _gaussEffectInsPosi.transform.position, Quaternion.identity);
        var bullet = _objectPool.GetObject(_bulletEffect, _gaussEffectInsPosi.transform.position, _gaussEffectInsPosi.transform.rotation);
        bullet.SetActive(false);
        if (_lockOn)
        {
            var aim = _target - bullet.transform.position;
            var look = Quaternion.LookRotation(aim);
            bullet.transform.rotation = look;
        }
        bullet.SetActive(true);
    }
}
