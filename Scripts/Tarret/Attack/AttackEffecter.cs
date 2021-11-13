using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffecter : MonoBehaviour
{
    /// <summary>衝撃波のエフェクト </summary>
    [SerializeField] GameObject _shockWaveEffect;
    [SerializeField] int _shockWaveMax;
    /// <summary>衝撃波の生成位置 </summary>
    [SerializeField] GameObject _shockWaveEffectInsPosi;
    [SerializeField] GameObject _objectPoolObj;

    /// <summary>廃熱エフェクトの生成位置 </summary>
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
    /// 衝撃波を徐々に大きくしていき、一定時間で消滅する
    /// </summary>
    public void InstanceShockWave()
    {
        _objectPool.GetObject(_shockWaveEffect, _shockWaveEffectInsPosi.transform.position, _shockWaveEffectInsPosi.transform.rotation);
    }

    /// <summary>
    /// 廃熱エフェクトの実体化から消滅までの管理をするメソッド
    /// </summary>
    public void InstanceWasteHeatEffect()
    {
        _objectPool.GetObject(_wasteHeatEffect, _wasteHeatEffectInsPosi.transform);
    }
}
