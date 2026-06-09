using UnityEngine;

public class EnemyBase : EntityBase
{
    private void Awake()
    {
        Setup();
    }

    protected override void Setup()
    {
        stats.maxHP = 100 + 50 * (stats.level - 1); // 기본(100) + 추가(50*(레벨-1))

        base.Setup();
    }
}
