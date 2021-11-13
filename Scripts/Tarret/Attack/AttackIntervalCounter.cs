using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tarret;

public class AttackIntervalCounter : MonoBehaviour
{
    public float attackIntervalTime = 1.4f;
    [SerializeField] GameObject screenLeftSliderObj;
    [SerializeField] GameObject screenRightSliderObj;
    Slider screenLeftSlider;
    Slider screenRightSlider;
    TarretAttacker tarretAttack;

    float m_time = 0;
    // Start is called before the first frame update
    void Start()
    {
        screenLeftSlider = screenLeftSliderObj.GetComponent<Slider>();
        screenRightSlider = screenRightSliderObj.GetComponent<Slider>();
        tarretAttack = GetComponent<TarretAttacker>();
    }

    public IEnumerator AttackIntervalCounterEnumerator()
    {
        var loop = true;
        while (loop)
        {
            m_time += Time.deltaTime;
            screenLeftSlider.value = Mathf.InverseLerp(attackIntervalTime, 0, m_time);
            screenRightSlider.value = Mathf.InverseLerp(attackIntervalTime, 0, m_time);
            if (m_time >= attackIntervalTime)
            {
                m_time = 0;
                loop = false;
                tarretAttack.EndAttack();
            }
            yield return null;
        }
            
    }
}
