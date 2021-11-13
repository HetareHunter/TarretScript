using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffecter : MonoBehaviour
{
    /// <summary>�Ռ��g�̃G�t�F�N�g </summary>
    [SerializeField] GameObject _shockWaveEffect;
    [SerializeField] int _shockWaveMax;
    /// <summary>�Ռ��g�̐����ʒu </summary>
    [SerializeField] GameObject _shockWaveEffectInsPosi;
    [SerializeField] GameObject _objectPoolObj;

    /// <summary>�p�M�G�t�F�N�g�̐����ʒu </summary>
    [SerializeField] GameObject _wasteHeatEffectInsPosi;
    [SerializeField] GameObject _wasteHeatEffect;
    [SerializeField] int _wasteHeatMax;
    ObjectPool _objectPool;

    // Start is called before the first frame update
    void Awake()
    {
        _objectPool = _objectPoolObj.GetComponent<ObjectPool>();

        _objectPool.CreatePool(_shockWaveEffect, _shockWaveMax);
        _objectPool.CreatePool(_wasteHeatEffect, _wasteHeatMax, _wasteHeatEffectInsPosi.transform);
    }

    /// <summary>
    /// �Ռ��g�����X�ɑ傫�����Ă����A��莞�Ԃŏ��ł���
    /// </summary>
    public void InstanceShockWave()
    {
        _objectPool.GetObject(_shockWaveEffect, _shockWaveEffectInsPosi.transform.position, _shockWaveEffectInsPosi.transform.rotation);
    }

    /// <summary>
    /// �p�M�G�t�F�N�g�̎��̉�������ł܂ł̊Ǘ������郁�\�b�h
    /// </summary>
    public void InstanceWasteHeatEffect()
    {
        _objectPool.GetObject(_wasteHeatEffect, _wasteHeatEffectInsPosi.transform);
    }
}
