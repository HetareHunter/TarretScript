using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tarret;

[RequireComponent(typeof(GaussFire))]
public class AttackRaycastManager : MonoBehaviour
{
    /// <summary>レイキャストの長さ </summary>
    [SerializeField] Vector3 maxRayDistance;
    [SerializeField] GameObject defaultRazerFinishPosition;

    /// <summary> 当たったオブジェクトの情報を入れておく変数 </summary>
    List<RaycastHit> _hitsEnemy;

    TarretAttacker _tarretAttacker;
    float _muzzleRadius;

    [Header("当たるとレーザーがそのオブジェクトを貫通しないレイヤー")]
    public LayerMask _noPenetrationLayer;
    public int _PeneLayerMaskNum;
    [SerializeField] GameObject tarret;

    GaussFire _gaussFire;

    // Start is called before the first frame update
    void Start()
    {
        _tarretAttacker = tarret.GetComponent<TarretAttacker>();
        _gaussFire = GetComponent<GaussFire>();

        //　弾の半径を取得
        _muzzleRadius = GetComponent<SphereCollider>().radius;
        _hitsEnemy = new List<RaycastHit>();

        int maskNum = _noPenetrationLayer;
        int layerNum = 0;
        while (maskNum > 0)
        {
            maskNum >>= 1;
            if (maskNum <= 0)
            {
                _PeneLayerMaskNum = layerNum;
                break;
            }
            layerNum++;
        }
    }

    void FixedUpdate()
    {
        RaySearchObject();
    }

    /// <summary>
    /// レイを飛ばして当たったオブジェクトが何なのかを判定する関数
    /// </summary>
    void RaySearchObject()
    {
        _hitsEnemy.Clear();
        //　飛ばす位置と飛ばす方向を設定
        Ray ray = new Ray(transform.position, transform.forward);

        //　Sphereの形でレイを飛ばしEnemy、GameManagerレイヤーのオブジェクトをm_hitsEnemyに入れる
        //RaycastAll系は取得したオブジェクトを一番遠いものから順に配列に格納していくので、0に近い要素数程遠いものになる
        var hits = Physics.SphereCastAll(ray, _muzzleRadius, maxRayDistance.z, LayerMask.GetMask("Enemy", "TutorialTarget"));
        for (int i = 0; i < hits.Length; i++)
        {
            _hitsEnemy.Add(hits[i]);
        }

        if (_hitsEnemy.Count > 0)
        {
            _tarretAttacker.ScreenChangeColor(true);

            var target = _hitsEnemy[_hitsEnemy.Count - 1].point;
            _gaussFire.SetTarget = target;
            //DebugUIBuilder.instance.AddLabel("x:" + target.x.ToString() + " y:" + target.y.ToString() + " z;" + target.z.ToString());
        }
        else
        {
            _tarretAttacker.ScreenChangeColor(false);
            _gaussFire.TargetUnlock = false;
        }
    }
}
