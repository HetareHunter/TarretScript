using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotate : MonoBehaviour
{
    [SerializeField] GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        //target = SpawnerManager.Instance.enemyTarget;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPositon = target.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetPositon - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
    }
}
