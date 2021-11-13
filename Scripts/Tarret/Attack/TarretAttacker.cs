using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Zenject;

namespace Tarret
{
    /// <summary>
    /// タレットの攻撃関係の処理をまとめているクラス
    /// 機能が集中しすぎてきたため機能を分散すること
    /// </summary>
    public class TarretAttacker : MonoBehaviour
    {
        AttackIntervalCounter attackInterval;
        AudioPlayer muzzleAudio;
        MagazineRotate magazineRotate;
        SightChanger sightChanger;
        GaussFire razerAttacker;
        AttackEffecter razerEffecter;
        [Inject]
        private IChangeSightColor changeSightColor;

        /// <summary> 攻撃可能かどうか判定する </summary>
        [NonSerialized] public bool attackable = true;

        [SerializeField] GameObject muzzle;
        [SerializeField] GameObject magazine;

        /// <summary> スクリーンに投影する照準についての変数 </summary>
        [SerializeField] GameObject sight;
        bool screenColorRed = false;

        private void Start()
        {
            muzzleAudio = muzzle.GetComponent<AudioPlayer>();
            magazineRotate = magazine.GetComponent<MagazineRotate>();
            sightChanger = sight.GetComponent<SightChanger>();
            attackInterval = GetComponent<AttackIntervalCounter>();
            razerAttacker = muzzle.GetComponent<GaussFire>();
            razerEffecter = GetComponent<AttackEffecter>();
        }

        public void ScreenChangeColor(bool raycastHit)
        {
            if (raycastHit)
            {
                //スクリーンのUI表示の色替え
                if (screenColorRed == false)
                {
                    sightChanger.ChangeRed();
                    changeSightColor.ChangeSliderFillRed();
                    screenColorRed = true;
                }
            }
            else
            {
                //スクリーンのUI表示の色替え
                if (screenColorRed == true)
                {
                    sightChanger.ChangeBase();
                    changeSightColor.ChangeSliderFillBase();
                    screenColorRed = false;
                }
            }
        }

        void EffectFadeTask()
        {
            magazineRotate.RotateMagazine();
        }

        /// <summary>
        /// 攻撃したとき、TarretCommandステートがAttackになったときに一度だけ呼ばれるメソッド。
        /// </summary>
        public void BeginAttack()
        {
            muzzleAudio.AudioPlay();
            razerAttacker.InstanceFireEffect();
            razerEffecter.InstanceWasteHeatEffect();
            razerEffecter.InstanceShockWave();

            IsAttackable(false);
            StartCoroutine(attackInterval.AttackIntervalCounterEnumerator());
            //attackInterval.countStart = true;
        }


        public void EndAttack()
        {
            EffectFadeTask();
            IsAttackable(true);
        }

        /// <summary>
        /// タレットの攻撃ができる状態にするかできない状態にするかの関数
        /// </summary>
        /// <param name="onAttack"></param>
        public void IsAttackable(bool onAttack)
        {
            attackable = onAttack;
        }
    }
}