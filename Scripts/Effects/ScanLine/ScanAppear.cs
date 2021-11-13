using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトの出現が終わっているかどうか
/// 基本的にEnemySpawnShaderを使っているマテリアルによって可視化され、
/// 全部見えるようになった状態を想定している
/// </summary>
public interface IAppearable
{
    public bool FinishAppear { get; set; }
    public void StartSpawn();
}

/// <summary>
/// EnemySpawnShaderを使っているマテリアルがアタッチされているオブジェクトを可視化するクラス
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
    /// SpawnMTの見えない部分と見える部分の境界を初期位置に戻す。つまり全部見えない状態に戻る
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
