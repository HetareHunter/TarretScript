using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の沸き方を管理するクラス
/// </summary>
public class BlockSpawnerManager : MonoBehaviour, ISpawnable
{
    [SerializeField] GameObject enemy;
    BlocksManager _blocksManager;
    private void Awake()
    {
        _blocksManager = enemy.GetComponent<BlocksManager>();
    }

    public void SpawnStart()
    {
        EnemySpawn();
    }

    public void SpawnEnd()
    {
        _blocksManager.ActivateBlocks();
    }

    public void ChangeEnemyNum(int num)
    {
    }


    public void EnemySpawn()
    {
        _blocksManager.ResetBlocks();
    }

    public void Reset()
    {
        _blocksManager.NonActivateBlocks();
    }
}
