using UnityEngine;

public class ProjectileTornado : ProjectileGlobal
{
    [SerializeField]
    private float attackRadius = 2f;        // 토네이도 공격 범위 반지름
    [SerializeField]
    private LayerMask targetLayer;
    private EntityBase target;

    public override void Setup(SkillBase skillBase, float damage)
    {
        base.Setup(skillBase, damage);
        // 토네이도는 필드를 이동하므로 MovementRigidbody2D 컴포넌트 사용
        movement2D = GetComponent<MovementRigidbody2D>();
        // 토네이도의 첫번째 목표 설정
        target = skillBase.Owner.Target;
    }

    public override void Process()
    {
        base.Process();

        if(target == null)
        {
            SearchClosestTarget();
            return;
        }

        // 목표(target) 위치로 이동
        movement2D.MoveTo((target.MiddlePoint - transform.position).normalized);

        // AttackRate 시간마다 토네이도 범위(attackRate)의 모든 적 공격
        if(Time.time - currentAttackRate > skillBase.GetStat(StatType.AttackRate).Value)
        {
            Collider2D[] entities = Physics2D.OverlapCircleAll(transform.position, attackRadius, targetLayer);
            for (int i = 0; i < entities.Length; i++) 
            {
                if (entities[i].TryGetComponent<EntityBase>(out var entity))
                {
                    TakeDamage(entity);
                }
            }

            currentAttackRate = Time.time;
        }

    }

    private void SearchClosestTarget()
    {
        float closestDistSqr = Mathf.Infinity;
        foreach (var entity in EnemySpawner.Enemies)
        {
            float distance = (entity.transform.position - transform.position).sqrMagnitude;
            if (distance < closestDistSqr)
            {
                closestDistSqr = distance;
                target = entity.GetComponent<EnemyBase>();
            }
        }
    }
}