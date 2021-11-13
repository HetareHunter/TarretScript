using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 振動処理拡張クラス
/// </summary>
public class VibrationExtension : SingletonMonoBehaviour<VibrationExtension>
{

    Coroutine leftViveCoroutine;
    Coroutine rightViveCoroutine;
    public void VibrateController(float duration, float frequency, float amplitude, OVRInput.Controller controller)
    {

        if (controller == OVRInput.Controller.LTouch)
        {
            leftViveCoroutine = StartCoroutine(VibrateForSeconds(duration, frequency, amplitude, controller));
        }
        else if (controller == OVRInput.Controller.RTouch)
        {
            rightViveCoroutine = StartCoroutine(VibrateForSeconds(duration, frequency, amplitude, controller));
        }
        else
        {
            return;
        }
    }

    public void VibrateStop(OVRInput.Controller controller)
    {
        if (controller == OVRInput.Controller.LTouch)
        {
            StopCoroutine(leftViveCoroutine);
            leftViveCoroutine = null;
        }
        else if (controller == OVRInput.Controller.RTouch)
        {
            StopCoroutine(rightViveCoroutine);
            rightViveCoroutine = null;
        }
        else
        {
            return;
        }
    }

    IEnumerator VibrateForSeconds(float duration, float frequency, float amplitude, OVRInput.Controller controller)
    {
        // 振動開始
        OVRInput.SetControllerVibration(frequency, amplitude, controller);
        
        yield return new WaitForSeconds(duration);

        // 振動終了
        OVRInput.SetControllerVibration(0, 0, controller);
    }
}
