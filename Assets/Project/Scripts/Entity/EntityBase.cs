using UnityEngine;

public enum DamageResult
{
    Applied,
    Evaded,
    Ignored
}
public abstract class EntityBase : MonoBehaviour
{
    [SerializeField]
    private EntityStats stats;
    [SerializeField]
    private Transform middlePoint;
    public EntityStats Stats => stats;
    public bool IsDead => Stats.CurrentHP != null && 
                    Mathf.Approximately(Stats.CurrentHP.Value, 0f);

    public EntityBase Target {  get; set; }

    public Vector3 MiddlePoint => middlePoint != null ? middlePoint.position : Vector3.zero;
    protected virtual void Setup()
    {
        Stats.CurrentHP.DefaultValue = Stats.GetStat(StatType.HP).Value;
    }

    public DamageResult TakeDamage(float damage)
    {
        if (IsDead) 
            return DamageResult.Ignored;

        // Evasion 스탯 확률로 회피
        if (Random.value < stats.GetStat(StatType.Evasion).Value)
            return DamageResult.Evaded;

        Stats.CurrentHP.DefaultValue -= damage;

        if( Mathf.Approximately(Stats.CurrentHP.Value, 0f) )
        {
            // 사망처리
            OnDie();    // Entity 사망 처리
        }
        return DamageResult.Applied;
    }
    protected abstract void OnDie();
}
