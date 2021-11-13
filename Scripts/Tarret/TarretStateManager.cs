using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Players;

namespace Tarret
{
    public enum TarretStateType
    {
        Idle,
        Attack,
        Rotate,
        Break,
    }

    /// <summary>
    /// タレットのステートを管理するクラス
    /// </summary>
    public class TarretStateManager : MonoBehaviour, ITarretStateChangeable
    {
        ///<summary>Tarretのhandleを握ったときに情報が格納される変数</summary>
        public HandleGrabbable _leftHandle;
        ///<summary>Tarretのhandleを握ったときに情報が格納される変数</summary>
        public HandleGrabbable _rightHandle;

        TarretAttacker _tarretAttacker;
        TarretRotater _tarretRotater;
        AnglePointer _anglePoint;
        TarretVitalManager _tarretVitalManager;

        [SerializeField] GameObject tarretAnglePoint;
        bool anglePointPlayOneShot = false;

        TarretStateType _tarretStateType = TarretStateType.Idle;
        ITarretStateExecutable _tarretStateExecutable;
        /// <summary>gameStaterのインスタンスのキャッシュ </summary>
        Dictionary<TarretStateType, ITarretStateExecutable> _tarretStateTypes = new Dictionary<TarretStateType, ITarretStateExecutable>();

        private void Start()
        {
            _tarretAttacker = GetComponent<TarretAttacker>();
            _tarretRotater = GetComponent<TarretRotater>();
            if (tarretAnglePoint != null)
            {
                _anglePoint = tarretAnglePoint.GetComponent<AnglePointer>();
            }

            if (GetComponent<TarretVitalManager>())
            {
                _tarretVitalManager = GetComponent<TarretVitalManager>();
            }

            _tarretStateTypes.Add(TarretStateType.Idle, new TarretIdle(this));
            _tarretStateTypes.Add(TarretStateType.Rotate, new TarretRotate(_tarretRotater));
            _tarretStateTypes.Add(TarretStateType.Attack, new TarretAttack(_tarretAttacker, _leftHandle, _rightHandle));
            _tarretStateTypes.Add(TarretStateType.Break, new TarretBreak(_tarretVitalManager));

            _tarretStateExecutable = _tarretStateTypes[_tarretStateType];
        }

        void FixedUpdate()
        {
            ExecuteState();
        }

        /// <summary>
        /// タレットが回転するかどうかを判定する
        /// </summary>
        public void JudgeRotateTarret()
        {
            //両手ともタレットのハンドルを握っているとき
            if (_leftHandle.IsGrabbed && _rightHandle.IsGrabbed)
            {
                ToRotate();
                if (anglePointPlayOneShot)
                {
                    _anglePoint.BeginGrabHandle();
                    anglePointPlayOneShot = false;
                }
            }
            else
            {
                anglePointPlayOneShot = true;
            }
        }

        public void ToIdle()
        {
            _tarretStateType = TarretStateType.Idle;
            _tarretStateExecutable = _tarretStateTypes[_tarretStateType];
            _tarretStateExecutable.EnterTarretState();
        }
        

        public void ToRotate()
        {
            _tarretStateType = TarretStateType.Rotate;
            _tarretStateExecutable = _tarretStateTypes[_tarretStateType];
            _tarretStateExecutable.EnterTarretState();
        }

        public void ToAttack()
        {
            _tarretStateType = TarretStateType.Attack;
            _tarretStateExecutable = _tarretStateTypes[_tarretStateType];
            _tarretStateExecutable.EnterTarretState();

            ToIdle();
        }

        public void ToBreak()
        {
            _tarretStateType = TarretStateType.Break;
            _tarretStateExecutable = _tarretStateTypes[_tarretStateType];
            _tarretStateExecutable.EnterTarretState();
        }

        public void ExecuteState()
        {
            _tarretStateExecutable.StateUpdate();
        }

        public TarretStateType GetTarretState()
        {
            return _tarretStateType;
        }
    }

    public class TarretIdle : ITarretStateExecutable
    {
        TarretStateManager _tarretStateManager;
        public TarretIdle(TarretStateManager tarretStateManager)
        {
            _tarretStateManager = tarretStateManager;
        }

        public void EnterTarretState() { }
        public void StateUpdate()
        {
            _tarretStateManager.JudgeRotateTarret();
        }
    }

    public class TarretRotate : ITarretStateExecutable
    {
        TarretRotater _tarretRotater;
        public TarretRotate(TarretRotater tarretRotater)
        {
            _tarretRotater = tarretRotater;
        }

        public void EnterTarretState() { }
        public void StateUpdate()
        {
            _tarretRotater.MoveManager();
        }
    }

    public class TarretAttack : ITarretStateExecutable
    {
        TarretAttacker _tarretAttacker;
        HandleGrabbable _leftHandle;
        HandleGrabbable _rightHandle;
        public TarretAttack(TarretAttacker tarretAttacker, HandleGrabbable leftHandleGrabbable, HandleGrabbable rightHandleGrabbable)
        {
            _tarretAttacker = tarretAttacker;
            _leftHandle = leftHandleGrabbable;
            _rightHandle = rightHandleGrabbable;
        }

        public void EnterTarretState()
        {
            if (_tarretAttacker.attackable)
            {
                _tarretAttacker.BeginAttack();
                _leftHandle.AttackVibe();
                _rightHandle.AttackVibe();
            }
        }
        public void StateUpdate() { }
    }

    public class TarretBreak : ITarretStateExecutable
    {
        TarretVitalManager _tarretVitalManager;
        public TarretBreak(TarretVitalManager tarretVitalManager)
        {
            _tarretVitalManager = tarretVitalManager;
        }

        public void EnterTarretState()
        {
            _tarretVitalManager.TarretDeath();
        }
        public void StateUpdate() { }
    }
}