using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksDeath : EnemyDeath
{
    public override void OnDead()
    {
        AddScore();
        Destroy(gameObject, 0);
    }

    public override void AddScore()
    {
    }
}
