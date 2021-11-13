using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ���I��d�z��ɂ���ăI�u�W�F�N�g�v�[�����s��
/// �C���X�^���X������prefab���i�[���郊�X�g�ƁA���̃��X�g���i�[����Dictionary�̓�d�z��ŊǗ�����
/// ������s�����ƂŁA��̃R���|�[�l���g�ł��̃V�[���̃I�u�W�F�N�g�v�[�����Ǘ��ł���
/// ���g������
/// ���̃R���|�[�l���g���ǂ����̋�̃I�u�W�F�N�g�ɃA�^�b�`��
/// �e�R���|�[�l���g���炱�̃R���|�[�l���g�̃N���X���Ăяo���B
/// �@CreatePool�Ńv�[��������Prefab�Ə�������o�^���A�e�q�֌W�̂��̂Ɛe�����̃I�u�W�F�N�g�ɕ����ė��p����
/// �A���ۂɃI�u�W�F�N�g���o��(Instantiate���܂�)������Ƃ���GetObject�֐����Ăяo���B
/// </summary>
public class ObjectPool : MonoBehaviour
{
    Dictionary<string, List<GameObject>> _poolObjectLists = new Dictionary<string, List<GameObject>>();
    List<GameObject> _poolObjList;

    /// <summary>
    /// �I�u�W�F�N�g�v�[�����X�g��o�^����
    /// </summary>
    /// <param name="prefabName">Dictionary�̃L�[</param>
    /// <param name="objList"></param>
    public void CreatePoolLists(string prefabName, List<GameObject> objList)
    {
        _poolObjectLists.Add(prefabName, objList);
    }

    // �I�u�W�F�N�g�v�[�����쐬
    public void CreatePool(GameObject prefabObj, int maxCount)
    {
        _poolObjList = new List<GameObject>();
        for (int i = 0; i < maxCount; i++)
        {
            var newObj = CreateNewObject(prefabObj, i + 1);
            newObj.SetActive(false);
            _poolObjList.Add(newObj);
        }
        CreatePoolLists(prefabObj.name, _poolObjList);
    }

    // �I�u�W�F�N�g�v�[�����쐬
    public void CreatePool(GameObject prefabObj, int maxCount, Transform parentObj)
    {
        _poolObjList = new List<GameObject>();
        for (int i = 0; i < maxCount; i++)
        {
            var newObj = CreateNewObject(prefabObj, parentObj, i + 1);
            newObj.SetActive(false);
            _poolObjList.Add(newObj);
        }
        CreatePoolLists(prefabObj.name, _poolObjList);
    }

    GameObject CreateNewObject(GameObject prefabObj, int nameIndex)
    {
        var newObj = Instantiate(prefabObj);
        newObj.name = prefabObj.name + nameIndex;

        return newObj;
    }

    GameObject CreateNewObject(GameObject prefabObj, Transform parentObj, int nameIndex)
    {
        var newObj = Instantiate(prefabObj, parentObj);
        newObj.name = prefabObj.name + nameIndex;

        return newObj;
    }

    public GameObject GetObject(GameObject prefabObj)
    {
        // �g�p���łȂ����̂�T���ĕԂ�
        foreach (var obj in _poolObjectLists[prefabObj.name])
        {
            if (obj.activeSelf == false)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // �S�Ďg�p����������V��������ĕԂ�
        var newObj = CreateNewObject(prefabObj, _poolObjectLists[prefabObj.name].Count + 1);
        newObj.SetActive(true);
        _poolObjectLists[prefabObj.name].Add(newObj);

        return newObj;
    }

    public GameObject GetObject(GameObject prefabObj, Transform parentObj)
    {
        foreach (var obj in _poolObjectLists[prefabObj.name])
        {
            if (obj.activeSelf == false)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        var newObj = CreateNewObject(prefabObj, parentObj, _poolObjectLists[prefabObj.name].Count + 1);
        newObj.SetActive(true);
        _poolObjectLists[prefabObj.name].Add(newObj);

        return newObj;
    }

    /// <summary>
    /// �v�[������I�u�W�F�N�g���E���グ�A�Ăяo�����ɓn��
    /// </summary>
    /// <param name="prefabObj"></param>
    /// <param name="position"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public GameObject GetObject(GameObject prefabObj, Vector3 position, Quaternion angle)
    {
        foreach (var obj in _poolObjectLists[prefabObj.name])
        {
            if (obj.activeSelf == false)
            {
                obj.transform.SetPositionAndRotation(position, angle);
                obj.SetActive(true);
                return obj;
            }
        }

        var newObj = CreateNewObject(prefabObj, _poolObjectLists[prefabObj.name].Count + 1);
        newObj.transform.SetPositionAndRotation(position, angle);
        newObj.SetActive(true);
        _poolObjectLists[prefabObj.name].Add(newObj);

        return newObj;
    }
}

