using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �^�[�Q�b�g�ł���V���G�b�g���N��������|�����肷��N���X�BDoTween�ŃA�j���[�V���������Ă���
/// �l�̃V���G�b�g��Position�������Ȃ�
/// </summary>
public class SilhouetteMover : MonoBehaviour, IMovableSilhouette
{
    Tweener _startTween;
    Sequence _silhouetteSequence;

    [SerializeField] Vector3[] _wayPoint;
    [Tooltip("�ړ����Ă��鎞��")]
    [SerializeField] float _moveDuration = 4.0f;
    [Tooltip("�����Ă��邾���̎���")]
    [SerializeField] float _standbyTime = 2.0f;
    float _activeTime;
    Vector3 startPosi;

    public float ActiveTime
    {
        get
        {
            return _activeTime;
        }
        set { }
    }

    /// <summary>
    /// ���݂̃X�^���h�̏��
    /// </summary>
    SilhouetteStandState _standState = SilhouetteStandState.Down;

    void Awake()
    {
        if (_wayPoint.Length == 0)
        {
            _activeTime = _standbyTime;
        }
        else
        {
            _activeTime = _moveDuration + _standbyTime * 2;//_standbyTime�͋N���オ�����Ƃ��A��������~�܂�������2��
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
        startPosi = transform.position;
        SetTweenPath();
    }

    private void Reset()
    {
        transform.position = startPosi;
    }

    /// <summary>
    /// �V���G�b�g�̉�]������]�����郁�\�b�h
    /// </summary>
    /// <param name="standState">���ꂩ�炱�̏�Ԃɂ������Ƃ���hikisuu
    /// </param>
    /// <param name="rotateTime"></param>
    public void StandSilhouette(SilhouetteStandState standState, float rotateTime)
    {
        if (_standState == SilhouetteStandState.Down && standState == SilhouetteStandState.Up)//�N���オ�鎞�̏���
        {
            transform.DORotate(new Vector3(-90, 0, 0), rotateTime, RotateMode.LocalAxisAdd)
                .SetEase(Ease.OutBounce);
        }
        else if (_standState == SilhouetteStandState.Up && standState == SilhouetteStandState.Down)//�|��鎞�̏���
        {
            DoKillTween();
            transform.DORotate(new Vector3(90, 0, 0), rotateTime, RotateMode.LocalAxisAdd)
                .OnComplete(() =>
                {
                    SetTweenPath();
                    Reset();
                })
                .SetEase(Ease.OutBounce);
        }
        _standState = standState;
    }

    /// <summary>
    /// �ړ��o�H�̓o�^�����đҋ@����
    /// </summary>
    void SetTweenPath()
    {
        if (_wayPoint.Length > 0) //�����ꍇ
        {
            _startTween = transform.DOLocalPath(_wayPoint, _moveDuration)
                .SetDelay(_standbyTime)
                .SetEase(Ease.Linear);

            _silhouetteSequence = DOTween.Sequence()
                .Append(_startTween)
                .AppendInterval(_standbyTime)
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject);
        }
    }

    void DoPlayTween()
    {
        _silhouetteSequence.Play();
    }

    void DoPauseTween()
    {
        _silhouetteSequence.Pause();
    }

    public void DoKillTween()
    {
        _startTween.Kill();
        _silhouetteSequence.Kill(false);
    }

    public void DoRestart()
    {
        if (_silhouetteSequence != null)
        {
            _silhouetteSequence.Restart();
        }
    }
}
