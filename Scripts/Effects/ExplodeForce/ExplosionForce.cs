using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionForce : MonoBehaviour
{
    [SerializeField] GameObject _muzzle;
    /// <summary>衝撃の物理的な力 </summary>
    [SerializeField] List<GameObject> _explodeForces = new List<GameObject>();
    int _explodeForceIndex = 0;
    [SerializeField] GameObject _explodeForceOrigin;

    // Start is called before the first frame update
    void Start()
    {
    }

    /// <summary>
    /// 攻撃が当たった時の吹き飛ばす力。オブジェクトを物理的に高速でぶつけている
    /// </summary>
    /// <param name="hitPosi"></param>
    public void ActiveExplosionForce(Vector3 hitPosi)
    {
        _explodeForces[_explodeForceIndex].transform.position = hitPosi;
        _explodeForces[_explodeForceIndex].transform.rotation = _muzzle.transform.rotation;
        _explodeForces[_explodeForceIndex].SetActive(true);

        _explodeForceIndex++;
        if (_explodeForceIndex >= _explodeForces.Count)
        {
            _explodeForceIndex = 0;
        }
    }
}
