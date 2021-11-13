using UnityEngine;

public abstract class EnemyDeath : MonoBehaviour
{
    abstract public void OnDead();
    abstract public void AddScore();

    [Tooltip("攻撃が貫通するかどうか")]
    /// <summary>
    /// 弾が貫通するかどうかのフラグ。敵は全て一撃必倒であるため、攻撃が当たった時のダメージ処理、効果も兼ねる
    /// </summary>
    [SerializeField] bool penetratable;
    public bool Penetratable
    {
        get
        {
            return penetratable;
        }
        set { } 
    }
}
