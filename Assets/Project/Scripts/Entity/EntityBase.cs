using UnityEngine;

public class EntityBase : MonoBehaviour
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

    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        Stats.CurrentHP.DefaultValue -= damage;

        if( Mathf.Approximately(Stats.CurrentHP.Value, 0f) )
        {
            // 사망처리
        }
    }
}
