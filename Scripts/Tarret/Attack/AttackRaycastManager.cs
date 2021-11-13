using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tarret;

[RequireComponent(typeof(GaussFire))]
public class AttackRaycastManager : MonoBehaviour
{
    /// <summary>���C�L���X�g�̒��� </summary>
    [SerializeField] Vector3 maxRayDistance;
    [SerializeField] GameObject defaultRazerFinishPosition;

    /// <summary> ���������I�u�W�F�N�g�̏������Ă����ϐ� </summary>
    List<RaycastHit> _hitsEnemy;

    TarretAttacker _tarretAttacker;
    float _muzzleRadius;

    [Header("������ƃ��[�U�[�����̃I�u�W�F�N�g���ђʂ��Ȃ����C���[")]
    public LayerMask _noPenetrationLayer;
    public int _PeneLayerMaskNum;
    [SerializeField] GameObject tarret;

    GaussFire _gaussFire;

    // Start is called before the first frame update
    void Start()
    {
        _tarretAttacker = tarret.GetComponent<TarretAttacker>();
        _gaussFire = GetComponent<GaussFire>();

        //�@�e�̔��a���擾
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
    /// ���C���΂��ē��������I�u�W�F�N�g�����Ȃ̂��𔻒肷��֐�
    /// </summary>
    void RaySearchObject()
    {
        _hitsEnemy.Clear();
        //�@��΂��ʒu�Ɣ�΂�������ݒ�
        Ray ray = new Ray(transform.position, transform.forward);

        //�@Sphere�̌`�Ń��C���΂�Enemy�AGameManager���C���[�̃I�u�W�F�N�g��m_hitsEnemy�ɓ����
        //RaycastAll�n�͎擾�����I�u�W�F�N�g����ԉ������̂��珇�ɔz��Ɋi�[���Ă����̂ŁA0�ɋ߂��v�f�����������̂ɂȂ�
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
