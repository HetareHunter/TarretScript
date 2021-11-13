using UnityEngine;

public abstract class EnemyDeath : MonoBehaviour
{
    abstract public void OnDead();
    abstract public void AddScore();

    [Tooltip("�U�����ђʂ��邩�ǂ���")]
    /// <summary>
    /// �e���ђʂ��邩�ǂ����̃t���O�B�G�͑S�Ĉꌂ�K�|�ł��邽�߁A�U���������������̃_���[�W�����A���ʂ����˂�
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
