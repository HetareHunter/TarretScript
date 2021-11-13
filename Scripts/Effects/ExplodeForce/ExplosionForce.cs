using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionForce : MonoBehaviour
{
    [SerializeField] GameObject _muzzle;
    /// <summary>�Ռ��̕����I�ȗ� </summary>
    [SerializeField] List<GameObject> _explodeForces = new List<GameObject>();
    int _explodeForceIndex = 0;
    [SerializeField] GameObject _explodeForceOrigin;

    // Start is called before the first frame update
    void Start()
    {
    }

    /// <summary>
    /// �U���������������̐�����΂��́B�I�u�W�F�N�g�𕨗��I�ɍ����łԂ��Ă���
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
