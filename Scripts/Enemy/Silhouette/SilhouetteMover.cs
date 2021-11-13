using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// ターゲットであるシルエットを起こしたり倒したりするクラス。DoTweenでアニメーションさせている
/// 人のシルエットはPositionが動かない
/// </summary>
public class SilhouetteMover : MonoBehaviour, IMovableSilhouette
{
    Tweener _startTween;
    Sequence _silhouetteSequence;

    [SerializeField] Vector3[] _wayPoint;
    [Tooltip("移動している時間")]
    [SerializeField] float _moveDuration = 4.0f;
    [Tooltip("立っているだけの時間")]
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
    /// 現在のスタンドの状態
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
            _activeTime = _moveDuration + _standbyTime * 2;//_standbyTimeは起き上がったとき、動いた後止まった時の2回
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
    /// シルエットの回転軸を回転させるメソッド
    /// </summary>
    /// <param name="standState">これからこの状態にしたいというhikisuu
    /// </param>
    /// <param name="rotateTime"></param>
    public void StandSilhouette(SilhouetteStandState standState, float rotateTime)
    {
        if (_standState == SilhouetteStandState.Down && standState == SilhouetteStandState.Up)//起き上がる時の処理
        {
            transform.DORotate(new Vector3(-90, 0, 0), rotateTime, RotateMode.LocalAxisAdd)
                .SetEase(Ease.OutBounce);
        }
        else if (_standState == SilhouetteStandState.Up && standState == SilhouetteStandState.Down)//倒れる時の処理
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
    /// 移動経路の登録をして待機する
    /// </summary>
    void SetTweenPath()
    {
        if (_wayPoint.Length > 0) //動く場合
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
