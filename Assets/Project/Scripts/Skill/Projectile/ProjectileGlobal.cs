using UnityEngine;

public class ProjectileGlobal : ProjectileBase
{
    [SerializeField]
    private Transform hitEffect;
    [SerializeField]
    private UIDamageText damageText;

    protected SkillBase skillBase;
    protected float currentDuration;
    protected float currentAttackRate = 0;
    protected float damage;

    public override void Setup(SkillBase skillBase, float damage)
    {
        this.skillBase = skillBase;
        this.damage = damage;
        currentDuration = Time.time;
    }

    /// <summary> 
    /// 지속 시간(Duration)이 있는 스킬만 base.Process() 호출한다.
    /// </summary>
    public override void Process()
    {
        // 지속 시간이 없는 스킬이 base.Process()를 실행하지 않도록 예외 처리
        if (skillBase.GetStat(StatType.Duration) == null)
            return;

        // 생성 시점(currentDuration)부터 StatType.Duration 지나면 발사체 삭제
        if(Time.time - currentDuration > skillBase.GetStat(StatType.Duration).Value)
        {
            Destroy(gameObject);
        }
    }
    protected void TakeDamage(EntityBase entity)
    {

        DamageResult result = entity.TakeDamage(damage);

        if (result == DamageResult.Ignored)
            return;

        if (hitEffect != null)
        {
            Instantiate(hitEffect, entity.MiddlePoint, Quaternion.identity);
        }
        if (damageText != null)
        {
            UIDamageText clone = Instantiate(damageText, entity.MiddlePoint, Quaternion.identity);
            string damageStr = result == DamageResult.Evaded ? "Miss" : damage.ToString("F0");
            Color color = result == DamageResult.Evaded ? Color.black : Color.white;
            clone.Setup(damageStr, color);
        }

       
    }
}