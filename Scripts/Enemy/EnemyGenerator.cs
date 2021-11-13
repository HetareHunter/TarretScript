using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour, ISpawnable
{
    public void Reset()
    {

    }
    public void ChangeEnemyNum(int num)
    {

    }
    public void SpawnStart()
    {

    }
    public void SpawnEnd()
    {

    }
}


public interface ISpawnable
{
    public void Reset();
    public void ChangeEnemyNum(int num);
    public void SpawnStart();
    public void SpawnEnd();
}