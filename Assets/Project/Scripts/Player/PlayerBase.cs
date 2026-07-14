using UnityEngine;
public class PlayerBase : EntityBase
{
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private FollowTarget targetMask;
    [SerializeField]
    private LevelData levelData;            // 인게임 레벨별 경험치 테이블 정보
    [SerializeField]
    private SkillSystem skillSystem;        // 레벨업 할 때 스킬을 배울 수 있도록

    private float expAmount = 2f;           // 매 프레임 흡수하는 경험치 양

    // 현재 플레이어가 이동 중인지
    // 스킬 사용 등 여러 곳에서 필요하므로 public 속성으로 정의
    public bool IsMoved { get; set; } = false;
    public float AccumulationExp { get; set; } = 0f;
    // 현재 스테이지에서 획득한 보석 개수
    public int GEM { get; private set; } = 0;
    
    private void Awake()
    {
        base.Setup();

        Stats.CurrentEXP.DefaultValue = 0f;
        Stats.CurrentEXP.OnValueChanged += IsLevelUP;
        Stats.GetStat(StatType.Experience).DefaultValue = levelData.MaxExperience[0];
    }
    private void Update()
    {
        if( Target == null ) targetMask.gameObject.SetActive( false );

        SearchTarget();
        Recovery();
        UpdateEXP();
    }

    protected override void OnDie()
    {
        gameController.GameOver();
    }
    private void SearchTarget()
    {
        float closestDistSqr = Mathf.Infinity;

        foreach( var entity in EnemySpawner.Enemies)
        {
            // 가장 가까운 대상을 찾으므로 sqrMagnitude 사용
            float distance = (entity.transform.position - transform.position).sqrMagnitude;
            if(distance < closestDistSqr)
            {
                closestDistSqr = distance;
                Target = entity.GetComponent<EntityBase>();
            }
        }

        if( Target != null)
        {
            targetMask.SetTarget(Target.transform);
            targetMask.transform.position = Target.transform.position;
            targetMask.gameObject.SetActive (true);
        }
    }

    private void Recovery()
    {
        // 체력 회복
        if (Stats.CurrentHP.Value < Stats.GetStat(StatType.HP).Value)
            Stats.CurrentHP.DefaultValue += Time.deltaTime * Stats.GetStat(StatType.HPRecovery).Value;
        else
            Stats.CurrentHP.DefaultValue = Stats.GetStat(StatType.HP).Value;
    }

    private void UpdateEXP()
    {
        if (Mathf.Approximately(AccumulationExp, 0f) ||
            skillSystem.IsSelectSkill == true)
            return;

        float getExp = AccumulationExp > expAmount ? expAmount : AccumulationExp;
        AccumulationExp -= getExp;                  // 축적 경험치에서 getEXP만큼 소모
        Stats.CurrentEXP.DefaultValue += getExp;    // 경험치를 getEXP만큼 증가

    }
    private void IsLevelUP(Stat stat, float prev, float current)
    {
        // 경험치가 최대가 아니면 return
        if (!Mathf.Approximately(Stats.CurrentEXP.Value, Stats.GetStat(StatType.Experience).Value))
            return;

        // 플레이어 레벨업(현재는 최대 레벨일 때 UI를 출력하거나 하지 않음)
        Stats.GetStat(StatType.Level).DefaultValue++;

        // 현재 경험치 설정(레벨업에 사용한 양만큼 감소)
        Stats.CurrentEXP.DefaultValue -= Stats.GetStat(StatType.Experience).Value;

        // 최대 경험치 설정
        if (Stats.GetStat(StatType.Level).Value < levelData.MaxExperience.Length)
            Stats.GetStat(StatType.Experience).DefaultValue = levelData.MaxExperience[(int)Stats.GetStat(StatType.Level).Value - 1];
        else
            Stats.GetStat(StatType.Experience).DefaultValue = levelData.MaxExperience[levelData.MaxExperience.Length - 1];

        // 레벨업 할 때 스킬을 선택할 수 있도록 스킬 선택 팝업 창 출력
        skillSystem.StartSelectSkill();
    }
    public void AddGem()
    {
        GEM++;
    }
}
