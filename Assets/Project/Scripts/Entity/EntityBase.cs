using UnityEngine;

public class EntityBase : MonoBehaviour
{
    [SerializeField]
    protected EntityStats stats;

    public EntityStats Stats => stats;
    public bool IsDead => stats.currentHP <= 0;

    protected virtual void Setup()
    {
        stats.currentHP = stats.maxHP;
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        stats.currentHP = stats.currentHP - damage > 0 ? stats.currentHP - damage : 0;

        if( stats.currentHP <= 0 )
        {
            // 사망처리
        }
    }
}
