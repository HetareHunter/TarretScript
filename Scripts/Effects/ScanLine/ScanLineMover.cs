using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanLineMover : MonoBehaviour
{
    [SerializeField] Vector3 newPosi;
    Vector3 startPosi;
    [SerializeField] Vector3 endPosi;
    float[] lineRanges;
    float scanWallSpeed = 30.0f;
    [SerializeField] Material[] scanLineMTs;
    public bool playScan = false;

    // Start is called before the first frame update
    void Start()
    {
        startPosi = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (playScan)
        {
            UpdateWallPosi();
        }
    }

    void UpdateWallPosi()
    {
        newPosi = transform.position;
        newPosi.z += scanWallSpeed * Time.deltaTime;
        transform.position = newPosi;

        UpdateScanLinePosi();
        if (newPosi.z >= endPosi.z)
        {
            FinishScan();
        }
    }

    void UpdateScanLinePosi()
    {
        for (int i = 0; i < scanLineMTs.Length; i++)
        {
            scanLineMTs[i].SetFloat("_ScanPosi", -newPosi.z  + lineRanges[i]);
        }
    }

    void GetStartScanLinePosi()
    {
        lineRanges = new float[scanLineMTs.Length];
        for (int i = 0; i < scanLineMTs.Length; i++)
        {
            lineRanges[i] = scanLineMTs[i].GetFloat("_LineRange");
        }
    }

    void FinishScan()
    {
        transform.position = startPosi; //スキャンの壁を初期位置に戻す
        newPosi = transform.position;
        UpdateScanLinePosi(); //スキャンラインシェーダ側も初期位置に戻す

        playScan = false;
    }

    public void StartScan()
    {
        playScan = true;

        GetStartScanLinePosi();
    }

#if UNITY_EDITOR

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartScan();
        }
    }

#endif
}
