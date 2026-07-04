using UnityEngine;

public class EnemyBase : EntityBase
{
    [SerializeField]
    private Transform hudPoint;
    [SerializeField]
    private GameObject uiPrefab;

    private EnemySpawner enemySpawner;

    private void Awake()
    {
        Setup();
    }

    protected override void Setup()
    {
        // 기본 체력은 DefaultValue에 할당하므로 추가 체력(BonusValue)만 설정
        Stats.GetStat(StatType.HP).BonusValue = 50 * (Stats.GetStat(StatType.Level).Value - 1);

        base.Setup();
    }

    public void Initialize(EnemySpawner enemySpawner, Transform parent)
    {
        this.enemySpawner = enemySpawner;
        GameObject clone = Instantiate(uiPrefab, parent);
        clone.transform.localScale = Vector3.one;
        clone.GetComponent<FollowTargetUI>().Setup(hudPoint);
        clone.GetComponent<UIHP>().Setup(this);
    }
    public override void OnDie()
    {
        // 적은 레벨업 하지 않으므로 적 경험치 스탯만큼 플레이어 경험치 증가
        (Target as PlayerBase).AccumulationExp += Stats.CurrentEXP.Value;
        // 적 본인(this) 사망 처리
        enemySpawner.Deactivate(this);
    }
}
