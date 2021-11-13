using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Hand
{
    Left,
    Right,
    None
}

public enum SelectableHand
{
    Left,
    Right,
    Both,
    None
}

namespace Players
{
    public interface ISelectable
    {
        /// <summary>
        /// 選択している手
        /// </summary>
        public Hand SelectHand { get; set; }
        /// <summary>
        /// 選択できる手
        /// </summary>
        public SelectableHand SelectableHand { get; set; }
        public void SelectHandle(bool isTouch, Hand hand);
        public void ChangeOutlineColor(bool isTouch);
        public void VanishOutline();
    }


    public class ObjectLaserPointer : MonoBehaviour
    {
        public Hand _hand;
        [SerializeField] GameObject defaultLineFinishPosi;
        [SerializeField] GameObject customHand;
        LineRenderer lineRenderer;
        /// <summary>
        /// 手から出るレーザー。レイを可視化しているだけのものなので始点と終点の2点の座標を格納する
        /// </summary>
        Vector3[] linePosition = new Vector3[2];
        RaycastHit _hit;
        [SerializeField] float maxRayDistance = 0.5f;
        ISelectable selectedObj;
        IGrabbable grabbable;

        [SerializeField] float grabBegin = 0.55f;
        [SerializeField] float grabEnd = 0.35f;
        bool _preGrab = false;

        /// <summary>
        /// レイで掴めるものを探索できるかどうかのフラグ。
        /// 何かを掴んでいる間は探索できないようにする
        /// </summary>
        public bool _searchable = true;

        [SerializeField] GameObject _handMesh;
        Renderer _handMeshRenderer;
        [SerializeField] Material _defaultHandMT;
        [SerializeField] Material _wireFrameMT;

        [SerializeField] GameObject _customOVRCameraRig;
        UserGuid _userGuid;
        bool _grabbableObjSelectBegin = true;

        // Start is called before the first frame update
        void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            _handMeshRenderer = _handMesh.GetComponent<Renderer>();
            _handMeshRenderer.material = _defaultHandMT;
            _userGuid = _customOVRCameraRig.GetComponent<UserGuid>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            RaySearchObject();
            CheckForGrabOrRelease();
        }

        /// <summary>
        /// レイを飛ばして当たったオブジェクトが何なのかを判定する関数
        /// </summary>
        void RaySearchObject()
        {
            if (!_searchable)
            {
                DrawLineRenderer(customHand.transform.position);
                return;
            }
            //　飛ばす位置と飛ばす方向を設定
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out _hit, maxRayDistance, LayerMask.GetMask("Tarret")))
            {
                DrawLineRenderer(_hit.point);
                ObjectSelectBegin();
                if (!JudgeSelectable(_hand)) selectedObj = null; //掴めないものであった場合nullにする

                if (selectedObj != null)//レーザーの触れたものがつかめるものである場合
                {
                    if (selectedObj.SelectHand == _hand || selectedObj.SelectHand == Hand.None)//選択している手がこのコンポーネントの手、あるいは選択している手がない場合
                    {
                        selectedObj.SelectHandle(true, _hand);
                        if (_grabbableObjSelectBegin)
                        {
                            GrabbableObjectSelectBegin(_hand);
                        }

                        if (grabbable == null)//トリガーを押せば掴むことができる状態にする
                        {
                            grabbable = _hit.transform.GetComponent<IGrabbable>();
                        }
                    }
                    else //選択している手がこのコンポーネントではない手である場合
                    {
                        selectedObj = null;
                    }
                }
            }
            else
            {
                ObjectSelectEnd();
                if (grabbable != null)
                {
                    grabbable = null;
                }
                DrawLineRenderer(defaultLineFinishPosi.transform.position);
            }
        }

        bool JudgeSelectable(Hand hand)
        {
            if (hand == Hand.Left)
            {
                if (selectedObj.SelectableHand == SelectableHand.Left || selectedObj.SelectableHand == SelectableHand.Both)
                {
                    return true;
                }
            }
            else if (hand == Hand.Right)
            {
                if (selectedObj.SelectableHand == SelectableHand.Right || selectedObj.SelectableHand == SelectableHand.Both)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 掴めるかどうか分からないがレイが触れることができるオブジェクトに触れたときに呼ばれる
        /// </summary>
        void ObjectSelectBegin()
        {
            if (selectedObj == null) //GetComponentを毎フレーム常に行うのは避けたい
            {
                selectedObj = _hit.transform.GetComponent<ISelectable>();
            }
        }

        void ObjectSelectEnd()
        {
            if (selectedObj != null)//レイが外れる瞬間に呼び出される処理
            {
                selectedObj.SelectHandle(false, Hand.None);
                selectedObj = null;
                GrabbableObjectSelectEnd(_hand);
            }
        }

        /// <summary>
        /// 掴めるオブジェクトに触れた瞬間の処理
        /// </summary>
        void GrabbableObjectSelectBegin(Hand hand)
        {
            _grabbableObjSelectBegin = false;
            _userGuid.SwicthHandMesh(true, hand);
        }

        void GrabbableObjectSelectEnd(Hand hand)
        {
            _grabbableObjSelectBegin = true;
            _userGuid.SwicthHandMesh(false, hand);
        }

        void DrawLineRenderer(Vector3 finishLine)
        {
            linePosition[0] = customHand.transform.position;
            linePosition[1] = finishLine;
            lineRenderer.SetPositions(linePosition);
        }

        void CheckForGrabOrRelease()
        {
            if (grabbable == null) return;

            if (_hand == Hand.Left)
            {
                if (OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger) >= grabBegin)
                {
                    _preGrab = true;
                    _searchable = false;
                    grabbable.GrabBegin(OVRInput.Controller.LTouch, transform);
                    _handMeshRenderer.material = _wireFrameMT;
                    _userGuid.SwicthHandMesh(false, _hand);
                }
                else if (OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger) <= grabEnd && _preGrab)
                {
                    _preGrab = false;
                    grabbable.GrabEnd();
                    grabbable = null;
                    _searchable = true;
                    _handMeshRenderer.material = _defaultHandMT;
                }
            }
            else if (_hand == Hand.Right)
            {
                if (OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) >= grabBegin)
                {
                    _preGrab = true;
                    _searchable = false;
                    grabbable.GrabBegin(OVRInput.Controller.RTouch, transform);
                    _handMeshRenderer.material = _wireFrameMT;
                    _userGuid.SwicthHandMesh(false, _hand);
                }
                else if (OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) <= grabEnd && _preGrab)
                {
                    _preGrab = false;
                    grabbable.GrabEnd();
                    grabbable = null;
                    _searchable = true;
                    _handMeshRenderer.material = _defaultHandMT;
                }
            }
        }
    }
}