using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �V���G�b�g�̃X�|�i�[�N���X(���Ԍo�߂ƃA�N�e�B�u�ȃV���G�b�g�I�u�W�F�N�g�̐��̏����ŐV���ɃV���G�b�g���N�����������s��)
/// </summary>
public class SilhouetteActivateManager : MonoBehaviour, ISpawnable
{
    IGameStateChangeable _gameStateChangable;
    [SerializeField] GameObject _gameManager;

    [Tooltip("���̎q�I�u�W�F�N�g�ɓ���āAsetActive��true�̃V���G�b�g�I�u�W�F�N�g���Q�[�����Ɏg�p�����")]
    public GameObject _registerSilhouettesParent;
    List<GameObject> _registerSilhouettes = new List<GameObject>();
    List<SilhouetteActivatior> _registerSilhouettesActivater = new List<SilhouetteActivatior>();
    Queue<SilhouetteActivatior> _silhouettes = new Queue<SilhouetteActivatior>();

    /// <summary> �����ɃA�N�e�B�u�ɂł���V���G�b�g�̐�</summary>
    int _activeSilhouetteNum = 0;
    [SerializeField] int _maxActiveSilhouetteNum = 3;

    /// <summary> �N���オ��܂ł̎��ԊԊu </summary>
    [SerializeField] float _spawnInterval = 5.0f;
    float _nowTime = 0;

    /// <summary> �����܂ł̎��Ԃ��o�߂������ǂ��� </summary>
    bool _onSpawnTimePassed = false;
    /// <summary> �����ėǂ��󋵂����Ǘ�����t���O </summary>
    bool _spawnable = false;
    /// <summary> �X�|�i�[���N�����Ă��邩�ǂ��� </summary>
    bool _onSpawn = false;
    /// <summary> 
    /// �Q�[�����I������܂łɋN������V���G�b�g�̐�
    /// �����V���G�b�g��2��ȏ�A�N�e�B�u�ɂ��Ȃ��B
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
        ChangeEnemyNum(1); //�G�̐���������

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

        if (num < 0)//�G���������ꍇ�ɃQ�[���I�����������
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
    /// �O��G�������Ă���ǂꂾ�����Ԃ��o�����̂����v������
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
    /// �I���ăQ�[���Ɏg���V���G�b�g��o�^����
    /// �����œo�^�����V���G�b�g�����Ԍo�߂ŋN�����A�������ł���悤�ɂȂ�
    /// </summary>
    /// <param name="registerSilhouettes">�Ώۂ̃V���G�b�g�̓C���X�y�N�^�[����ݒ肷��</param>
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
