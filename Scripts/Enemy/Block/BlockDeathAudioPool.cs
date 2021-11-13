using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDeathAudioPool : MonoBehaviour
{
    ObjectPool _objectPool;
    GameObject _objectPoolObj;
    [Header("オブジェクトプールに設定するもの")]
    [SerializeField] GameObject _deathAudioPrefab;
    [SerializeField] int _deathAudioPrefabNum;
    // Start is called before the first frame update
    void Start()
    {
        _objectPoolObj = GameObject.Find("ObjectPool");
        _objectPool = _objectPoolObj.GetComponent<ObjectPool>();
        _objectPool.CreatePool(_deathAudioPrefab, _deathAudioPrefabNum);
    }

    public void CallDeadSound(Vector3 soundPos)
    {
        _objectPool.GetObject(_deathAudioPrefab, soundPos, Quaternion.identity);
    }
}
