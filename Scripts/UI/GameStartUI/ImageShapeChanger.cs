using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ImageShapeChanger : MonoBehaviour
{
    [SerializeField] Image TouchCountImage;
    [SerializeField] Image[] startUIImage;
    [SerializeField] TextMeshPro[] _countText;//複数のUIに対応するための配列

    public void LoadTouchImage(float toStartTime, float toStartLimitTime)
    {
        //ロード画面の画像が丸になっていくことでロード時間の可視化をする
        TouchCountImage.fillAmount = toStartTime / toStartLimitTime;
    }

    public void LoadCountImage(float per)
    {
        //毎秒のイメージの変化
        for (int i = 0; i < startUIImage.Length; i++)//演出するスクリーンの数だけ繰り返す
        {
            startUIImage[i].fillAmount = per;
        }
    }

    public void WriteScreenText(string input)
    {
        for (int i = 0; i < _countText.Length; i++)//登録したUIテキストの全てに変更を加える
        {
            _countText[i].text = input;
        }
    }
}
