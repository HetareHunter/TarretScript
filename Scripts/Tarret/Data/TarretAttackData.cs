using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "_Scripts/Create TarretAttackData")]

public class TarretAttackData : ScriptableObject
{
    [Header("爆発の存在する時間")]
    public float explodeExistTime = 3.0f;

    [Header("攻撃時のコントローラの振動時間")]
    public float attackVibeDuration = 0.6f;
    [Header("攻撃時のコントローラの振動数")]
    public float attackVibeFrequency = 0.2f;
    [Header("攻撃時のコントローラの振幅")]
    public float attackVibeAmplitude = 0.5f;
}
