using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シルエットのスポナークラス(時間経過とアクティブなシルエットオブジェクトの数の条件で新たにシルエットを起こす処理を行う)
/// </summary>
public class SilhouetteActivateManager : MonoBehaviour, ISpawnable
{
    IGameStateChangeable _gameStateChangable;
    [SerializeField] GameObject _gameManager;

    [Tooltip("この子オブジェクトに入れて、setActiveがtrueのシルエットオブジェクトがゲーム中に使用される")]
    public GameObject _registerSilhouettesParent;
    List<GameObject> _registerSilhouettes = new List<GameObject>();
    List<SilhouetteActivatior> _registerSilhouettesActivater = new List<SilhouetteActivatior>();
    Queue<SilhouetteActivatior> _silhouettes = new Queue<SilhouetteActivatior>();

    /// <summary> 同時にアクティブにできるシルエットの数</summary>
    int _activeSilhouetteNum = 0;
    [SerializeField] int _maxActiveSilhouetteNum = 3;

    /// <summary> 起き上がるまでの時間間隔 </summary>
    [SerializeField] float _spawnInterval = 5.0f;
    float _nowTime = 0;

    /// <summary> 沸くまでの時間が経過したかどうか </summary>
    bool _onSpawnTimePassed = false;
    /// <summary> 沸いて良い状況かを管理するフラグ </summary>
    bool _spawnable = false;
    /// <summary> スポナーを起動しているかどうか </summary>
    bool _onSpawn = false;
    /// <summary> 
    /// ゲームが終了するまでに起動するシルエットの数
    /// 同じシルエットを2回以上アクティブにしない。
    /// </summary>
    int _maxActivatedSilhouetteNum;
    int _activatedSilhouetteNum = 0;

    public int ActiveSilhouetteNum
    {
        get
        {
            return _activeSilhouetteNum;
        }
        set
        {
            _activeSilhouetteNum += value;
        }
    }

    public bool OnSpawn
    {
        get
        {
            return _onSpawn;
        }
        set
        {
            _onSpawn = value;
        }
    }
    private void Awake()
    {
        _gameStateChangable = _gameManager.GetComponent<IGameStateChangeable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        JudgeSpawn();
        var registerSilhouetteChildcount = _registerSilhouettesParent.transform.childCount;
        for (int i = 0; i < registerSilhouetteChildcount; i++)
        {
            var setObj= _registerSilhouettesParent.transform.GetChild(i).gameObject;
            if (setObj.activeSelf)
            {
                _registerSilhouettes.Add(setObj);
            }
        }

        foreach (var item in _registerSilhouettes)
        {
            _registerSilhouettesActivater.Add(item.GetComponentInChildren<SilhouetteActivatior>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_onSpawn)
        {
            if (_activeSilhouetteNum < _maxActiveSilhouetteNum)
            {
                CalculateSpawnTime();
            }
            JudgeSpawn();

            if (_spawnable)
            {
                ActivateSilhouette();
            }
        }
    }

    public void ActivateSilhouette()
    {
        _activatedSilhouetteNum++;
        Debug.Log("_activatedSilhouetteNum : " + _activatedSilhouetteNum);
        ChangeEnemyNum(1); //敵の数が増える

        _silhouettes.Enqueue(_silhouettes.Peek());
        _silhouettes.Dequeue().ActivateSilhouette();

        _nowTime = 0;
        _onSpawnTimePassed = false;
        _spawnable = false;
    }

    public void Reset()
    {
        _activatedSilhouetteNum = 0;
        _activeSilhouetteNum = 0;
        _nowTime = 0;
        _onSpawnTimePassed = false;
        foreach (var item in _silhouettes)
        {
            item.Reset();
        }
    }
    public void ChangeEnemyNum(int num)
    {
        _activeSilhouetteNum += num;
        Debug.Log("EnemyNum : " + _activeSilhouetteNum);
        if (_activatedSilhouetteNum >= _maxActivatedSilhouetteNum)
        {
            _onSpawn = false;
        }

        if (num < 0)//敵が減った場合にゲーム終了判定をする
        {
            JudgeGameFinish();
        }
    }
    public void SpawnStart()
    {
        _onSpawn = true;
        RegisterSilhouettes(_registerSilhouettesActivater);
        JudgeSpawn();
    }
    public void SpawnEnd()
    {
        _onSpawn = false;
        Reset();
    }
    void JudgeSpawn()
    {
        if (_activeSilhouetteNum < _maxActiveSilhouetteNum && _onSpawnTimePassed)
        {
            _spawnable = true;
        }
        else
        {
            _spawnable = false;
        }
    }

    /// <summary>
    /// 前回敵が沸いてからどれだけ時間が経ったのかを計測する
    /// </summary>
    void CalculateSpawnTime()
    {
        _nowTime += Time.deltaTime;
        if (_nowTime > _spawnInterval)
        {
            _onSpawnTimePassed = true;
            _nowTime = 0;
        }
    }

    /// <summary>
    /// 的当てゲームに使うシルエットを登録する
    /// ここで登録したシルエットが時間経過で起動し、撃つ事ができるようになる
    /// </summary>
    /// <param name="registerSilhouettes">対象のシルエットはインスペクターから設定する</param>
    void RegisterSilhouettes(List<SilhouetteActivatior> registerSilhouettes)
    {
        _maxActivatedSilhouetteNum = registerSilhouettes.Count;
        registerSilhouettes = registerSilhouettes.OrderBy(x => Guid.NewGuid()).ToList();
        for (int i = 0; i < _registerSilhouettes.Count; i++)
        {
            _silhouettes.Enqueue(registerSilhouettes[i]);
        }
    }

    void JudgeGameFinish()
    {
        if (!_onSpawn && _activeSilhouetteNum <= 0)
        {
            _gameStateChangable.ToEnd();
        }
    }
}
