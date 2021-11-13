using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UserGuid : MonoBehaviour
{
    [SerializeField] GameObject _handMesh_L;
    [SerializeField] GameObject _handMesh_R;
    [SerializeField] GameObject _handController_L;
    [SerializeField] GameObject _handController_R;

    SkinnedMeshRenderer _handMeshRenderer_L;
    SkinnedMeshRenderer _handMeshRenderer_R;
    Renderer _controllerMeshRenderer_L;
    Renderer _controllerMeshRenderer_R;
    MaterialPropertyBlock _LControllerMpb;
    MaterialPropertyBlock _RControllerMpb;

    [SerializeField] bool _onStartControllerMesh = false;

    string _onEmissionID = "Boolean_373b9f6827e4408dba5a032c280ab463";

    void Awake()
    {
        _handMeshRenderer_L = _handMesh_L.GetComponent<SkinnedMeshRenderer>();
        _handMeshRenderer_R = _handMesh_R.GetComponent<SkinnedMeshRenderer>();
        _controllerMeshRenderer_L = _handController_L.GetComponent<Renderer>();
        _controllerMeshRenderer_R = _handController_R.GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _LControllerMpb = new MaterialPropertyBlock();
        _RControllerMpb = new MaterialPropertyBlock();

        if (_onStartControllerMesh)
        {
            _controllerMeshRenderer_L.enabled = true;
            _controllerMeshRenderer_R.enabled = true;
            _handMeshRenderer_L.enabled = false;
            _handMeshRenderer_R.enabled = false;
        }
        else
        {
            _controllerMeshRenderer_L.enabled = false;
            _controllerMeshRenderer_R.enabled = false;
            _handMeshRenderer_L.enabled = true;
            _handMeshRenderer_R.enabled = true;
        }
    }

    public void SwicthHandMesh(bool useControllerMesh, Hand hand)
    {
        if (useControllerMesh) //手がコントローラになる
        {
            if (hand == Hand.Left)
            {
                _handMeshRenderer_L.enabled = false;
                _controllerMeshRenderer_L.enabled = true;
            }
            else if (hand == Hand.Right)
            { 
                _handMeshRenderer_R.enabled = false;
                _controllerMeshRenderer_R.enabled = true;
            }
        }
        else //コントローラが手になる
        {
            if (hand == Hand.Left)
            {
                _handMeshRenderer_L.enabled = true;
                _controllerMeshRenderer_L.enabled = false;
            }
            else if (hand == Hand.Right)
            {
                _handMeshRenderer_R.enabled = true;
                _controllerMeshRenderer_R.enabled = false;
            }
        }
    }

    /// <summary>
    /// 光らせるボタンを選択する
    /// </summary>
    public void UseEmissionAButton(GameObject button)
    {
        if (button.GetComponent<Button>().interactable)
        {
            _RControllerMpb.SetFloat(Shader.PropertyToID(_onEmissionID), 1.0f);
            _controllerMeshRenderer_R.SetPropertyBlock(_RControllerMpb);
        }
    }

    public void NoUseEmissionAButton()
    {
        _RControllerMpb.SetFloat(Shader.PropertyToID(_onEmissionID), 0.0f);
        _controllerMeshRenderer_R.SetPropertyBlock(_RControllerMpb);
    }
}
