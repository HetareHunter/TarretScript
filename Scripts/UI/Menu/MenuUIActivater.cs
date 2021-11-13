using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Players;

namespace MenuUI
{
    /// <summary>
    /// メニューUIの子のcanvasクラスが付いたオブジェクトの表示のオンオフを切り替えるクラス
    /// </summary>
    public class MenuUIActivater : MonoBehaviour
    {
        GameObject SceneMovePanel;
        [SerializeField] GameObject GameFinishPanel;
        GameObject UIhelpers;
        LineRenderer lineRenderer;
        [SerializeField] GameObject _handPointerObj_L;
        [SerializeField] GameObject _handPointerObj_R;

        ObjectLaserPointer _objectLaserPointer_L;
        ObjectLaserPointer _objectLaserPointer_R;

        [SerializeField] GameObject _customOVRCameraRig;
        UserGuid _userGuid;

        private void Awake()
        {
            _objectLaserPointer_L = _handPointerObj_L.GetComponent<ObjectLaserPointer>();
            _objectLaserPointer_R = _handPointerObj_R.GetComponent<ObjectLaserPointer>();
            _userGuid = _customOVRCameraRig.GetComponent<UserGuid>();
        }

        // Start is called before the first frame update
        void Start()
        {
            if (SceneMovePanel == null)
            {
                SceneMovePanel = GameObject.Find("SceneMovePanel");
                SceneMovePanel.SetActive(false);
            }
            if (UIhelpers == null)
            {
                UIhelpers = GameObject.Find("MyUIHelpers");
            }
            lineRenderer = UIhelpers.GetComponentInChildren<LineRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (OVRInput.GetDown(OVRInput.Button.Start))
            {
                ActivateMenu();
            }
        }

        public void ActivateMenu()
        {
            if (!SceneMovePanel.activeSelf)
            {
                SceneMovePanel.SetActive(true);
                lineRenderer.enabled = true;

                _objectLaserPointer_L._searchable = false;
                _objectLaserPointer_R._searchable = false;

                _userGuid.SwicthHandMesh(true, Hand.Left);
                _userGuid.SwicthHandMesh(true, Hand.Right);
            }
            else
            {
                SceneMovePanel.SetActive(false);
                lineRenderer.enabled = false;

                _objectLaserPointer_L._searchable = true;
                _objectLaserPointer_R._searchable = true;

                _userGuid.SwicthHandMesh(false, Hand.Left);
                _userGuid.SwicthHandMesh(false, Hand.Right);
            }
        }

        public void ActivateTutorialFinishPanel()
        {
            if (!GameFinishPanel.activeSelf)
            {
                GameFinishPanel.SetActive(true);
            }
            else
            {
                GameFinishPanel.SetActive(false);
            }
        }
    }
}