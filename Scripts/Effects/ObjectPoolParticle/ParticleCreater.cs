using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticleCreater : MonoBehaviour
{
    public int _particleObjMax = 10;
    public GameObject _ObejctPoolObj;
    protected ObjectPool _objectPool;


    protected virtual void Awake()
    {
        _objectPool = _ObejctPoolObj.GetComponent<ObjectPool>();
    }
    public virtual void InstanceParticle(GameObject _particlePrefab)
    {
        var poolParticle = _objectPool.GetObject(_particlePrefab);
    }
}