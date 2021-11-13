using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �I�u�W�F�N�g�̏o�����I����Ă��邩�ǂ���
/// ��{�I��EnemySpawnShader���g���Ă���}�e���A���ɂ���ĉ�������A
/// �S��������悤�ɂȂ�����Ԃ�z�肵�Ă���
/// </summary>
public interface IAppearable
{
    public bool FinishAppear { get; set; }
    public void StartSpawn();
}

/// <summary>
/// EnemySpawnShader���g���Ă���}�e���A�����A�^�b�`����Ă���I�u�W�F�N�g����������N���X
/// </summary>
public class ScanAppear : MonoBehaviour
{
    Vector3 appearLinePosi;
    [SerializeField] Vector3 startAppearPosi;
    [SerializeField] Vector3 endAppearPosi;
    float[] lineRanges;
    [SerializeField] float scanWallSpeed = 1.0f;
    [SerializeField] Material[] SpawnMTs;
    public bool playScan = false;
    string _scanPosi = "Vector1_bc6a38dab71149e392ba24144467bb94";
    string _lineRange = "Vector1_73826083ca9b46dbb89f50e79b0c5527";

    public bool FinishAppear { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        GetStartAppearLinePosi();
        ResetApeearLinePosi();
        appearLinePosi = startAppearPosi;
        FinishAppear = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playScan)
        {
            UpdateAppearLinePosi();
        }
    }

    void UpdateAppearLinePosi()
    {
        appearLinePosi.y -= scanWallSpeed * Time.deltaTime;
        UpdateMTAppearLinePosi();

        if (appearLinePosi.y <= endAppearPosi.y)
        {
            FinishScan();
        }
    }

    void UpdateMTAppearLinePosi()
    {
        for (int i = 0; i < SpawnMTs.Length; i++)
        {
            SpawnMTs[i].SetFloat(_scanPosi, appearLinePosi.y);
        }
    }

    void GetStartAppearLinePosi()
    {
        lineRanges = new float[SpawnMTs.Length];
        for (int i = 0; i < SpawnMTs.Length; i++)
        {
            lineRanges[i] = SpawnMTs[i].GetFloat(_lineRange);
        }
    }

    public void StartSpawn()
    {
        playScan = true;
        FinishAppear = false;
        GetStartAppearLinePosi();
    }

    void FinishScan()
    {
        playScan = false;
        FinishAppear = true;
    }

    /// <summary>
    /// SpawnMT�̌����Ȃ������ƌ����镔���̋��E�������ʒu�ɖ߂��B�܂�S�������Ȃ���Ԃɖ߂�
    /// </summary>
    public void ResetApeearLinePosi()
    {
        appearLinePosi = startAppearPosi;
        UpdateMTAppearLinePosi();
        playScan = false;
        FinishAppear = false;
    }

#if UNITY_EDITOR

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            StartSpawn();
        }
    }

#endif
}
