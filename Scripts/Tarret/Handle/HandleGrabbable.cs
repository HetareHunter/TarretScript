using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public enum HandleSide
{
    Left,
    Right
}


namespace Players
{
    [RequireComponent(typeof(HandFixer))]
    [RequireComponent(typeof(HandleInput))]
    [RequireComponent(typeof(HandleVibe))]
    /// <summary>
    /// 実際にハンドルを握ることができるコライダー部分にコンポーネントを付ける
    /// 手についているクラスが処理を考えるのではなく握ったオブジェクトが処理を行う方向で組み立てる
    /// </summary>
    public class HandleGrabbable : MonoBehaviour, IGrabbable, ISelectable
    {
        [SerializeField] TarretAttackData tarretData;
        /// <summary>
        /// 握っている手が左か右かのステート
        /// </summary>
        OVRInput.Controller _currentController;
        /// <summary>
        /// 握っているかどうか
        /// </summary>
        bool _isGrabbed;
        /// <summary>
        /// 握ることのできるかどうか
        /// </summary>
        public bool _allowOffhandGrab = true;

        [Inject]
        ITarretStateChangeable _tarretState;
        [SerializeField] GameObject anglePointerObj;
        [SerializeField] Color startColor;
        Color _vanishingColor = new Color(0, 0, 0, 0);
        [SerializeField] Color selectedColor;
        /// <summary>
        /// 手のレイが当たっているかどうか
        /// </summary>
        bool _isSelect = false;
        bool _isSelectMoment = false;
        Hand _selectHand = Hand.None;
        HandlePositionResetter _returnPosition;
        AnglePointer _anglePointer;

        [SerializeField] GameObject handleObj;
        Renderer _handleRenderer;

        HandleVibe _handleVibe;
        HandleInput _handleInput;
        HandFixer _handFixer;
        
        public HandleSide _handle;
        public SelectableHand _selectableHand;

        /// <summary> 触れた時の振動の大きさ </summary>
        [SerializeField] float touchFrequeency = 0.3f;
        /// <summary> 触れた時の振動の周波数 </summary>
        [SerializeField] float touchAmplitude = 0.3f;
        /// <summary> 触れた時の振動の時間 </summary>
        [SerializeField] float touchVibeDuration = 0.2f;

        /// <summary>
        /// 手でつかんだ瞬間のフラグ
        /// </summary>
        bool _handleGrabMoment = false;

        Transform _grabbedHandTransform;

        public Hand SelectHand
        {
            get
            {
                return _selectHand;
            }
            set { }
        }

        public SelectableHand SelectableHand
        {
            get
            {
                return _selectableHand;
            }
            set { }
        }

        public bool IsGrabbed
        {
            get
            {
                return _isGrabbed;
            }
            private set { }
        }

        void Start()
        {
            if (anglePointerObj != null)
            {
                _anglePointer = anglePointerObj.GetComponent<AnglePointer>();
            }

            _handleRenderer = handleObj.GetComponent<Renderer>();

            _handleVibe = GetComponent<HandleVibe>();
            _handleInput = GetComponent<HandleInput>();
            _handFixer = GetComponent<HandFixer>();
            _returnPosition = GetComponent<HandlePositionResetter>();
        }
        void FixedUpdate()
        {
            GrabMethod();
        }

        /// <summary>
        /// 掴む処理
        /// </summary>
        private void GrabMethod()
        {
            if (_isGrabbed) //握っている間の処理
            {
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, _currentController) && _handle == HandleSide.Right)
                {
                    _handleInput.Attack();
                }

                if (_handle == HandleSide.Left)
                {
                    _handleInput.CartMove(OVRInput.Get(OVRInput.RawAxis2D.LThumbstick));
                }


                _handFixer.FixHand(); //手のメッシュの位置をハンドルの位置に固定し続けている
                FollowHand(_grabbedHandTransform);

                if (!_handleGrabMoment)//手を握った瞬間の処理
                {
                    VanishOutline();
                    _allowOffhandGrab = false;
                    _handleGrabMoment = true;
                }
            }
            else //離している間の処理
            {
                if (_handleGrabMoment)//手を離した瞬間の処理
                {
                    GrabEnd();
                }
            }
        }

        /// <summary>
        /// ハンドルのコリジョン部分を手のレイで当てて、中指のトリガーを引くことで握る処理を始めるメソッド
        /// </summary>
        /// <param name="controller"></param>
        public void GrabBegin(OVRInput.Controller controller, Transform transform)
        {
            _currentController = controller;
            _grabbedHandTransform = transform;
            _isGrabbed = true;
        }

        /// <summary>
        /// 手の中指トリガーを離したとき1回だけ呼び出されるメソッド
        /// </summary>
        public void GrabEnd()
        {
            _handFixer.ReleseHand();
            _currentController = OVRInput.Controller.None;

            _returnPosition.Released();
            ChangeOutlineColor(_isSelect);

            _tarretState.ToIdle();

            _grabbedHandTransform = null;
            _anglePointer.isAdjust = false;
            _handleGrabMoment = false;
            _allowOffhandGrab = true;
            _isGrabbed = false;
        }

        /// <summary>
        /// 手から放たれ続けているレイがオブジェクトに触れているときの処理
        /// </summary>
        /// <param name="isSelect">レイが触れた時trueにし、レイが外れた時、falseにする</param>
        /// <param name="hand">どちらの手から放たれたレイで取得するか</param>
        public void SelectHandle(bool isSelect, Hand hand)
        {
            _isSelect = isSelect;
            _selectHand = hand;

            if (!_isSelectMoment && isSelect)//触れた瞬間の処理
            {
                SelectEnter(isSelect, hand);
            }
            else if (_isSelectMoment && !isSelect)
            {
                SelectExit(isSelect, hand);
            }
        }

        /// <summary>
        /// レイがオブジェクトに触れた瞬間の処理
        /// </summary>
        /// <param name="isSelect"></param>
        /// <param name="hand"></param>
        void SelectEnter(bool isSelect, Hand hand)
        {
            _isSelectMoment = isSelect;
            if (hand == Hand.Left)
            {
                //握ったときにcurrentControllerにどちらのコントローラかの情報が入るので、触れたときの振動処理は
                //currentControllerを引数に使えない
                _handleVibe.Vibrate(touchVibeDuration, touchFrequeency, touchAmplitude, OVRInput.Controller.LTouch);
            }
            else if (hand == Hand.Right)
            {
                _handleVibe.Vibrate(touchVibeDuration, touchFrequeency, touchAmplitude, OVRInput.Controller.RTouch);
            }

            ChangeOutlineColor(_isSelect);
        }

        /// <summary>
        /// 触れたレイがオブジェクトから離れた瞬間の処理
        /// </summary>
        /// <param name="isSelect"></param>
        public void SelectExit(bool isSelect, Hand hand)
        {
            _selectHand = hand;
            _isSelectMoment = isSelect;
            ChangeOutlineColor(_isSelect);
        }

        public void AttackVibe()
        {
            if (_handleVibe != null && _currentController != OVRInput.Controller.None)
            {
                _handleVibe.Vibrate(tarretData.attackVibeDuration, tarretData.attackVibeFrequency,
                    tarretData.attackVibeAmplitude, _currentController);
            }
        }

        /// <summary>
        /// 手がハンドルに触れただけで、握ってはいないときに色を変える
        /// </summary>
        /// <param name="isSelect">触れているかどうか</param>
        public void ChangeOutlineColor(bool isSelect)
        {
            if (isSelect)
            {
                _handleRenderer.materials[1].SetColor("_OutlineColor", selectedColor);
            }
            else
            {
                _handleRenderer.materials[1].SetColor("_OutlineColor", startColor);
            }
        }

        /// <summary>
        /// ハンドルを握ったときに、ハンドルのアウトラインを透過する
        /// </summary>
        public void VanishOutline()
        {
            _handleRenderer.materials[1].SetColor("_OutlineColor", _vanishingColor);
        }

        void FollowHand(Transform handTransform)
        {
            transform.SetPositionAndRotation(handTransform.position, handTransform.rotation);
        }
    }
}
