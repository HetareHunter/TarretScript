using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 動的二重配列によってオブジェクトプールを行う
/// インスタンス化するprefabを格納するリストと、そのリストを格納するDictionaryの二重配列で管理する
/// これを行うことで、一つのコンポーネントでそのシーンのオブジェクトプールを管理できる
/// ※使い方※
/// このコンポーネントをどこかの空のオブジェクトにアタッチし
/// 各コンポーネントからこのコンポーネントのクラスを呼び出す。
/// ①CreatePoolでプールしたいPrefabと初期数を登録し、親子関係のものと親無しのオブジェクトに分けて利用する
/// ②実際にオブジェクトを出現(Instantiateも含む)させるときはGetObject関数を呼び出す。
/// </summary>
public class ObjectPool : MonoBehaviour
{
    Dictionary<string, List<GameObject>> _poolObjectLists = new Dictionary<string, List<GameObject>>();
    List<GameObject> _poolObjList;

    /// <summary>
    /// オブジェクトプールリストを登録する
    /// </summary>
    /// <param name="prefabName">Dictionaryのキー</param>
    /// <param name="objList"></param>
    public void CreatePoolLists(string prefabName, List<GameObject> objList)
    {
        _poolObjectLists.Add(prefabName, objList);
    }

    // オブジェクトプールを作成
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

    // オブジェクトプールを作成
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
        // 使用中でないものを探して返す
        foreach (var obj in _poolObjectLists[prefabObj.name])
        {
            if (obj.activeSelf == false)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // 全て使用中だったら新しく作って返す
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
    /// プールからオブジェクトを拾い上げ、呼び出し元に渡す
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

