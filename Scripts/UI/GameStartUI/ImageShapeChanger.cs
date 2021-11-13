using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ImageShapeChanger : MonoBehaviour
{
    [SerializeField] Image TouchCountImage;
    [SerializeField] Image[] startUIImage;
    [SerializeField] TextMeshPro[] _countText;//������UI�ɑΉ����邽�߂̔z��

    public void LoadTouchImage(float toStartTime, float toStartLimitTime)
    {
        //���[�h��ʂ̉摜���ۂɂȂ��Ă������ƂŃ��[�h���Ԃ̉���������
        TouchCountImage.fillAmount = toStartTime / toStartLimitTime;
    }

    public void LoadCountImage(float per)
    {
        //���b�̃C���[�W�̕ω�
        for (int i = 0; i < startUIImage.Length; i++)//���o����X�N���[���̐������J��Ԃ�
        {
            startUIImage[i].fillAmount = per;
        }
    }

    public void WriteScreenText(string input)
    {
        for (int i = 0; i < _countText.Length; i++)//�o�^����UI�e�L�X�g�̑S�ĂɕύX��������
        {
            _countText[i].text = input;
        }
    }
}
