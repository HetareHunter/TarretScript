using System.Collections;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum SieldState
{
    Full,
    Damaged,
    Brake,
    Recovery,
}

namespace Tarret
{
    public class TarretVitalManager : MonoBehaviour
    {
        [SerializeField] TarretVitalData TarretVitalData;

        [SerializeField] float tarretDamageCoefficient = 0.95f;
        [SerializeField] float sieldDamageCoefficient = 0.9f;

        [SerializeField] Slider tarretHPSlider;
        float tarretHP;

        [SerializeField] Slider sieldHPSlider;
        float sieldHP;
        [SerializeField] float recoveryTime = 1.0f;
        float currentRecoveryTime = 0;
        [SerializeField] float sieldRecoverySpeed = 2.0f;

        Sequence sieldRecoverySequence;
        /// <summary>
        /// 現状アタッチしているタレットのステートを知りたいのでインターフェースではなくクラス型を指定する
        /// </summary>
        TarretStateManager tarretStateManager;

        public SieldState sieldState;
        IGameStateChangeable gameStateChangable;
        [SerializeField] GameObject gameManager;

        /// <summary>攻撃を受けているときtrueになる </summary>

        // Start is called before the first frame update
        void Start()
        {
            tarretHP = TarretVitalData.TarretMaxHP;
            sieldHP = TarretVitalData.TarretMaxSield;
            tarretStateManager = GetComponent<TarretStateManager>();
            gameStateChangable = gameManager.GetComponent<IGameStateChangeable>();
        }

        private void Update()
        {
            if (tarretStateManager.GetTarretState() != TarretStateType.Break)
            {
                CalculataTarretVital();
            }
            
        }

        private void CalculataTarretVital()
        {
            sieldHPSlider.value = sieldHP / TarretVitalData.TarretMaxSield;
            if (sieldState == SieldState.Damaged || sieldState == SieldState.Brake)
            {
                currentRecoveryTime += Time.deltaTime;
                if (currentRecoveryTime > recoveryTime)
                {
                    currentRecoveryTime = 0;
                    SieldRecovery();
                }
            }
        }

        /// <summary>
        /// Fullのstateではなにもしない
        /// </summary>
        /// <param name="next"></param>
        void ChangeSieldState(SieldState next)
        {
            sieldState = next;
            switch (next)
            {
                case SieldState.Full:
                    break;
                case SieldState.Damaged:
                    currentRecoveryTime = 0;
                    break;
                case SieldState.Brake:
                    break;
                case SieldState.Recovery:
                    //SieldRecovery();
                    break;
                default:
                    break;
            }
        }

        public void TarretDamage(float damage)
        {
            currentRecoveryTime = 0;
            
            tarretHP -= (damage * tarretDamageCoefficient);
            tarretHPSlider.value = tarretHP / TarretVitalData.TarretMaxHP;
            if (tarretHP <= 0)
            {
                //tarretStateManager.ChangeTarretState(TarretStateType.Break);
                tarretStateManager.ToBreak();
            }

            Debug.Log("TarretVitalData.TarretHP : " + tarretHP);
        }

        public void SieldDamage(float damage)
        {
            sieldRecoverySequence.Kill(); //シールド回復の中断
            sieldHP -= (damage * sieldDamageCoefficient);

            if (sieldHP <= 0)
            {
                ChangeSieldState(SieldState.Brake);
            }
            else
            {
                ChangeSieldState(SieldState.Damaged);
            }
        }

        void SieldRecovery()
        {
            ChangeSieldState(SieldState.Recovery);
            sieldRecoverySequence = DOTween.Sequence()
                .OnStart(() =>
                {
                    //Debug.Log("シールドオン！");
                }
                ).Append(
                DOTween.To(
                () => sieldHP,
                (x) => sieldHP = x,
                TarretVitalData.TarretMaxSield,
                sieldRecoverySpeed)
                .SetEase(Ease.Linear)
                .OnComplete(() => ChangeSieldState(SieldState.Full))
                );
        }

        /// <summary>
        /// ゲームの敗北条件
        /// </summary>
        public void TarretDeath()
        {
            currentRecoveryTime = 0;
            tarretHP = 0;
            sieldHP = 0;
            ChangeSieldState(SieldState.Brake);

            //gameStateChangable.FinishGame(false);
        }

        public void ResetTarretVital()
        {
            tarretHP = TarretVitalData.TarretMaxHP;
            tarretHPSlider.value = tarretHP;
            sieldHP = TarretVitalData.TarretMaxSield;
            sieldHPSlider.value = sieldHP;
            currentRecoveryTime = 0;
            ChangeSieldState(SieldState.Full);
        }

        private void OnTriggerEnter(Collider other)
        {
            // レイヤー名を取得して layerName に格納
            string layerName = LayerMask.LayerToName(other.gameObject.layer);
            // レイヤー名がEnemyBullet以外の時は何も行わない
            if (layerName != "EnemyBullet") return;

            float damage = other.transform.GetComponent<BulletMove>().power;
            if (sieldState == SieldState.Brake)
            {
                TarretDamage(damage);
            }
            else
            {
                SieldDamage(damage);
            }
        }
    }
}