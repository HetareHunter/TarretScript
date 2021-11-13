using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// DissolveShader�̃}�e���A�����t���Ă���I�u�W�F�N�g�ɃA�^�b�`����B
/// ���b�V�����n���Ă������o�𐧌䂷��X�N���v�g
/// </summary>
public class MeshDissolver : MonoBehaviour
{
    [SerializeField] float _dissolveSpeed = 0.2f;
    MaterialPropertyBlock _material;
    MeshRenderer _meshRenderer;
    BlockDeath _blockDeath;
    [SerializeField] float _dissolveDelay = 0.5f;
    bool _onDissolve = false;
    string _dissolveThresholdstr = "Vector1_7e113bfd69c44598824716b9f5574a47";
    bool _onAvtivate = false;

    public bool OnActivate
    {
        get
        {
            return _onAvtivate;
        }
        set
        {
            _onAvtivate = value;
        }
    }

    public void Reset()
    {
        DoActivate();
    }

    // Start is called before the first frame update
    void Start()
    {
        _blockDeath = GetComponent<BlockDeath>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _material = new MaterialPropertyBlock();
    }

    void UpdateDissolveMaterial(float threshold)
    {
        _material.SetFloat(_dissolveThresholdstr, threshold);
        _meshRenderer.SetPropertyBlock(_material);
    }

    public void IsPlayDissolve(bool onPlay)
    {
        StartCoroutine(Dissolve(_dissolveDelay));
    }

    /// <summary>
    /// �����I�Ƀu���b�N���\���ɂ���Ƃ��g�p����B�u���b�N�Q�[�����^�C���A�b�v�ŏI��������A�r���I�������Ƃ��A�c���Ă����u���b�N��
    /// ���̊֐��Ŕ�\���ɂ��AActiveSelf��false�ɂ͂��Ȃ��B�i�T�E���h�G�t�F�N�g���c���������߁j
    /// </summary>
    public void DoNonActive()
    {
        UpdateDissolveMaterial(1.0f);
    }

    void DoActivate()
    {
        UpdateDissolveMaterial(0.0f);
    }

    IEnumerator Dissolve(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        var threshold = 0.0f;
        while (threshold < 1.0f)
        {
            threshold += _dissolveSpeed * Time.deltaTime;
            UpdateDissolveMaterial(threshold);
            _blockDeath.IsRigidBodyKinematicEnabled(true);
            yield return null;
        }
    }
}
