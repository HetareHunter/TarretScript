using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksManager : MonoBehaviour
{
    BlockDeath[] _enemyBlockDeath;
    private void Awake()
    {
        _enemyBlockDeath = GetComponentsInChildren<BlockDeath>();
    }
    private void Start()
    {
        NonActivateBlocks();
    }

    public void ResetBlocks()
    {
        foreach (var item in _enemyBlockDeath)
        {
            item.Reset();
        }
    }

    public void ActivateBlocks()
    {
        foreach (var item in _enemyBlockDeath)
        {
            item.DoActivate();
        }
    }

    public void NonActivateBlocks()
    {
        foreach (var item in _enemyBlockDeath)
        {
            item.DoNonActivate();
        }
    }
}
