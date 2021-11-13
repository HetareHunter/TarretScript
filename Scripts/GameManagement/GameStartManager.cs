using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Manager
{
    /// <summary>
    /// ゲームスタートをするまでの時間管理を行うクラス
    /// ゲーム開始までに、手でGameStartCanvasに触れてgameStateがStartになるまでの時間と
    /// gameStateがStartになってからPlayになるまでの時間の2つの時間を管理する
    /// </summary>
    public class GameStartManager : MonoBehaviour
    {
        [SerializeField] GameObject _gameManager;
        [SerializeField] GameObject spawnerManager;
        ScanAppear _scanAppear;
        IGameStateChangeable _gameStateChangeable;
        bool _onLeftHand = false;
        bool _onRightHand = false;
        bool _onHand = false;

        /// <summary>
        /// ゲームのスタートが確定するフラグ
        /// </summary>
        bool _onStart = false;

        /// <summary> 触れた時の振動の大きさ </summary>
        [SerializeField] float touchFrequeency = 0.3f;
        /// <summary> 触れた時の振動の周波数 </summary>
        [SerializeField] float touchAmplitude = 0.3f;

        float _toStartTime = 0;
        [SerializeField] float _toStartLimitTime = 2.0f;

        /// <summary>
        /// 1秒固定、1秒ごとにUIのカウントを３，２，１と変化させたいので変数を二つ用意した
        /// </summary>
        [SerializeField] float _toPlayCountDownTime = 3.0f;
        float _toPlayTimeCountDownFirstNum;

        /// <summary>
        /// スクリーンに投影するゲーム開始の待ち時間
        /// </summary>
        int _toPlayTimeCountScreenNum = 3;
        const float ToPlayScreenNumAdd = 0.999999f;

        Animator _gameStartUIAnim;

        [SerializeField] GameObject changeColorObj;
        ColorManager _colorManager;
        BoxCollider _boxCollider;
        ImageShapeChanger _imageShapeChanger;

        private void Start()
        {
            _gameStartUIAnim = GetComponent<Animator>();
            _colorManager = changeColorObj.GetComponent<ColorManager>();
            _gameStateChangeable = _gameManager.GetComponent<IGameStateChangeable>();
            _boxCollider = GetComponent<BoxCollider>();
            _scanAppear = spawnerManager.GetComponent<ScanAppear>();
            _imageShapeChanger = GetComponent<ImageShapeChanger>();

            _toPlayTimeCountScreenNum = (int)(_toPlayCountDownTime + ToPlayScreenNumAdd);
            _toPlayTimeCountDownFirstNum = _toPlayCountDownTime;
        }

        private void Update()
        {
            OnHandJudge();
            if (_onHand)
            {
                _imageShapeChanger.LoadTouchImage(_toStartTime, _toStartLimitTime);
                ToStartCount();
            }
        }

        public void Reset()
        {
            _toPlayCountDownTime = _toPlayTimeCountDownFirstNum;
            _toPlayTimeCountScreenNum = (int)(_toPlayCountDownTime + ToPlayScreenNumAdd);
            ResetScreen();
            if (_scanAppear != null)
            {
                _scanAppear.ResetApeearLinePosi();
            }
            ActivateGameStart(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("RHand"))
            {
                _onRightHand = true;
                VibrationExtension.Instance.VibrateController(_toStartLimitTime, touchFrequeency, touchAmplitude, OVRInput.Controller.RTouch);
            }
            else if (other.gameObject.CompareTag("LHand"))
            {
                _onLeftHand = true;
                VibrationExtension.Instance.VibrateController(_toStartLimitTime, touchFrequeency, touchAmplitude, OVRInput.Controller.LTouch);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("RHand"))
            {
                _onRightHand = false;
                VibrationExtension.Instance.VibrateStop(OVRInput.Controller.RTouch);
                OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
            }
            else if (other.gameObject.CompareTag("LHand"))
            {
                _onLeftHand = false;
                VibrationExtension.Instance.VibrateStop(OVRInput.Controller.LTouch);
                OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
            }
        }

        void OnHandJudge()
        {
            if (_onLeftHand || _onRightHand)
            {
                _onHand = true;
            }
            else
            {
                _onHand = false;
                _toStartTime = 0; //時間の初期化
                _imageShapeChanger.LoadTouchImage(_toStartTime, _toStartLimitTime); //UIの初期化
            }
        }

        /// <summary>
        /// タッチパネルに触れてからゲーム開始確定までの時間をカウントする
        /// GameStateの Idle->Start の段階
        /// </summary>
        void ToStartCount()
        {
            _toStartTime += Time.deltaTime;
            if (_toStartTime > _toStartLimitTime)
            {
                _toStartTime = 0;
                GameStart();
            }
        }

        /// <summary>
        /// GameStateの Start->Play の段階
        /// </summary>
        void ToPlayCount()
        {
            _toPlayCountDownTime -= Time.deltaTime;
            var preTimeCountScreenNum = _toPlayTimeCountScreenNum;
            if (_toPlayCountDownTime > 0)
            {
                _toPlayTimeCountScreenNum = (int)(_toPlayCountDownTime + ToPlayScreenNumAdd);
            }
            else
            {
                _toPlayTimeCountScreenNum = (int)_toPlayCountDownTime;
            }

            if (preTimeCountScreenNum != _toPlayTimeCountScreenNum)
            {
                _imageShapeChanger.WriteScreenText(_toPlayTimeCountScreenNum.ToString());
            }
            _imageShapeChanger.LoadCountImage(_toPlayCountDownTime % 1);

            if (_toPlayTimeCountScreenNum <= 0)
            {
                _gameStateChangeable.ToPlay();
                _colorManager.ToStartColor();

                _imageShapeChanger.WriteScreenText("Start!");
                _onStart = false;
            }
        }

        public void ResetScreen()
        {
            _imageShapeChanger.WriteScreenText("Ready?");
            _imageShapeChanger.LoadCountImage(1.0f);
            ActivateGameStart(true);
        }

        public void ChangeAnim()
        {
            _gameStartUIAnim.SetTrigger("StateChange");
        }

        public void GameStart()
        {
            _gameStateChangeable.ToStart(); //ゲーム開始
            _onStart = true;
            StartCoroutine(ToPlayCoroutine());

            _imageShapeChanger.WriteScreenText(_toPlayTimeCountScreenNum.ToString());
            _onRightHand = false;
            _onLeftHand = false;
            _boxCollider.enabled = false;
            
            _colorManager.ToChangeColor();
            ChangeAnim();
            if (_scanAppear != null)
            {
                _scanAppear.StartSpawn();
            }
        }

        public void GameEnd()
        {
            ChangeAnim();
            _imageShapeChanger.WriteScreenText("Finish!");
        }

        void ActivateGameStart(bool isActive)
        {
            _boxCollider.enabled = isActive;
        }

        IEnumerator ToPlayCoroutine()
        {
            var loop = true;
            while (loop)
            {
                if (_scanAppear != null)
                {
                    if (_scanAppear.FinishAppear)//オブジェクトの出現が終わっているならばゲームプレイステートへのカウントが始まる
                    {
                        ToPlayCount();
                    }
                }
                else//出現させる演出のあるオブジェクトがない場合は即カウントを始める
                {
                    ToPlayCount();
                }
                if (!_onStart) loop = false;
                yield return null;
            }
        }
    }
}