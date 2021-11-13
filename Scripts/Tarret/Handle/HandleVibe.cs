using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Players
{
    public class HandleVibe : MonoBehaviour
    {

        public void Vibrate(float duration, float frequency, float amplitude, OVRInput.Controller currentController)
        {
            VibrationExtension.Instance.VibrateController(
                        duration, frequency, amplitude, currentController);
        }
    }
}