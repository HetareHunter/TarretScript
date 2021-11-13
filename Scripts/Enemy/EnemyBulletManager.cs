using System.Collections;
using System.Collections.Generic;
using UnityEngine;

delegate void BulletLaunchDelegate();

public class EnemyBulletManager : MonoBehaviour
{
    [SerializeField] int bulletNum = 30;
    [SerializeField] GameObject originBullet;
    List<GameObject> bullets = new List<GameObject>();
    int bulletIndex = 0;

    [SerializeField] float maxAttackInterval = 1.0f;
    [SerializeField] float minAttackInterval = 0.1f;
    float attackIntarval;
    float currentIntervalTime = 0;
    bool attackable = false;
    BulletMove[] bulletMove;
    public bool deathEnemy = false;
    /// <summary>
    /// 弾道にばらつきを与える際のばらつき
    /// </summary>
    [SerializeField] float maxLaunchAngle = 0.3f;
    [SerializeField] float xAxisCoefficient = 1.0f;
    [SerializeField] float yAxisCoefficient = 1.0f;
    [SerializeField] float zAxisCoefficient = 1.0f;

    Quaternion launchAngle;

    BulletLaunchDelegate bulletLaunchMethod;
    [SerializeField] bool isRandomAngleBullet = false;

    private void Awake()
    {
        if (!isRandomAngleBullet)
        {
            bulletLaunchMethod = new BulletLaunchDelegate(FireBullet);
        }
        else
        {
            bulletLaunchMethod = new BulletLaunchDelegate(RandomAngleFireBullet);
            launchAngle = new Quaternion();
        }
        bulletMove = new BulletMove[bulletNum];
        for (int i = 0; i < bulletNum; i++)
        {
            bullets.Add(Instantiate(originBullet, transform.position, Quaternion.identity));
            bulletMove[i] = bullets[i].GetComponent<BulletMove>();
            bullets[i].SetActive(false);
        }

        SetAttackIntarvalTime();
    }

    private void Update()
    {
        if (attackable)
        {
            bulletLaunchMethod();
            SetAttackIntarvalTime();
            currentIntervalTime = 0;
            attackable = false;
        }
        else
        {
            CountIntarvalTime();
        }
    }

    /// <summary>
    /// 弾を前方に撃ちだす場合
    /// </summary>
    void FireBullet()
    {
        if (deathEnemy) return;
        bullets[bulletIndex].transform.position = transform.position;
        bullets[bulletIndex].transform.rotation = transform.rotation;
        bulletMove[bulletIndex].Fire();

        bullets[bulletIndex].SetActive(true);
        bulletIndex++;
        if (bulletIndex >= bullets.Count)
        {
            bulletIndex = 0;
        }
    }


    /// <summary>
    /// 弾を前方にばらつきを持たせて撃ちだす場合
    /// </summary>
    void RandomAngleFireBullet()
    {
        if (deathEnemy) return;
        bullets[bulletIndex].transform.position = transform.position;
        //bullets[bulletIndex].transform.rotation = transform.rotation;
        bullets[bulletIndex].transform.rotation = SetLaunchAngle();
        bulletMove[bulletIndex].Fire();

        bullets[bulletIndex].SetActive(true);
        bulletIndex++;
        if (bulletIndex >= bullets.Count)
        {
            bulletIndex = 0;
        }
    }

    void CountIntarvalTime()
    {
        currentIntervalTime += Time.deltaTime;
        if (currentIntervalTime > attackIntarval)
        {
            attackable = true;
        }
    }

    void SetAttackIntarvalTime()
    {
        attackIntarval = Random.Range(minAttackInterval, maxAttackInterval);
    }

    public void OnDead()
    {
        deathEnemy = true;
        foreach (var item in bullets)
        {
            if (item == null) continue;
            Destroy(item, 5.0f);
        }
        bulletMove = null;
        bullets = null;
    }

    Quaternion SetLaunchAngle()
    {
        float x = Random.Range(-maxLaunchAngle, maxLaunchAngle);
        float y = Random.Range(-maxLaunchAngle, maxLaunchAngle);
        float z = Random.Range(-maxLaunchAngle, maxLaunchAngle);
        launchAngle = transform.rotation;
        launchAngle.x += x * xAxisCoefficient;
        launchAngle.y += y * yAxisCoefficient;
        launchAngle.z += z * zAxisCoefficient;
        //launchAngle.z = transform.rotation.z;
        return launchAngle;
    }
}
