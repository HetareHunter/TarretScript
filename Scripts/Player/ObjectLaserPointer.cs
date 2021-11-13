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
        /// �I�����Ă����
        /// </summary>
        public Hand SelectHand { get; set; }
        /// <summary>
        /// �I���ł����
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
        /// �肩��o�郌�[�U�[�B���C���������Ă��邾���̂��̂Ȃ̂Ŏn�_�ƏI�_��2�_�̍��W���i�[����
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
        /// ���C�Œ͂߂���̂�T���ł��邩�ǂ����̃t���O�B
        /// ������͂�ł���Ԃ͒T���ł��Ȃ��悤�ɂ���
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
        /// ���C���΂��ē��������I�u�W�F�N�g�����Ȃ̂��𔻒肷��֐�
        /// </summary>
        void RaySearchObject()
        {
            if (!_searchable)
            {
                DrawLineRenderer(customHand.transform.position);
                return;
            }
            //�@��΂��ʒu�Ɣ�΂�������ݒ�
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out _hit, maxRayDistance, LayerMask.GetMask("Tarret")))
            {
                DrawLineRenderer(_hit.point);
                ObjectSelectBegin();
                if (!JudgeSelectable(_hand)) selectedObj = null; //�͂߂Ȃ����̂ł������ꍇnull�ɂ���

                if (selectedObj != null)//���[�U�[�̐G�ꂽ���̂����߂���̂ł���ꍇ
                {
                    if (selectedObj.SelectHand == _hand || selectedObj.SelectHand == Hand.None)//�I�����Ă���肪���̃R���|�[�l���g�̎�A���邢�͑I�����Ă���肪�Ȃ��ꍇ
                    {
                        selectedObj.SelectHandle(true, _hand);
                        if (_grabbableObjSelectBegin)
                        {
                            GrabbableObjectSelectBegin(_hand);
                        }

                        if (grabbable == null)//�g���K�[�������Β͂ނ��Ƃ��ł����Ԃɂ���
                        {
                            grabbable = _hit.transform.GetComponent<IGrabbable>();
                        }
                    }
                    else //�I�����Ă���肪���̃R���|�[�l���g�ł͂Ȃ���ł���ꍇ
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
        /// �͂߂邩�ǂ���������Ȃ������C���G��邱�Ƃ��ł���I�u�W�F�N�g�ɐG�ꂽ�Ƃ��ɌĂ΂��
        /// </summary>
        void ObjectSelectBegin()
        {
            if (selectedObj == null) //GetComponent�𖈃t���[����ɍs���͔̂�������
            {
                selectedObj = _hit.transform.GetComponent<ISelectable>();
            }
        }

        void ObjectSelectEnd()
        {
            if (selectedObj != null)//���C���O���u�ԂɌĂяo����鏈��
            {
                selectedObj.SelectHandle(false, Hand.None);
                selectedObj = null;
                GrabbableObjectSelectEnd(_hand);
            }
        }

        /// <summary>
        /// �͂߂�I�u�W�F�N�g�ɐG�ꂽ�u�Ԃ̏���
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