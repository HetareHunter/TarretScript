using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathImpactTorus : MonoBehaviour
{
    float deathTime = 3.0f;
    [SerializeField] TarretAttackData tarretAttackData;
    private void Start()
    {
        deathTime = tarretAttackData.explodeExistTime;
    }
    private void OnEnable()
    {
        Invoke("FadeImpact", deathTime);
    }

    void FadeImpact()
    {
        gameObject.SetActive(false);
    }
}
