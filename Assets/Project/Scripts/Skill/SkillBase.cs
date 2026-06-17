using UnityEngine;

public abstract class SkillBase
{
    protected SkillTemplate skillTemplate;      // 스킬 정보
    protected PlayerBase owner;                 // 스킬 소유자
    protected int currentLevel = 0;             // 현재 스킬 레벨

    // 외부에서 접근할 스킬 정보를 Get 속성으로 정의
    public string SkillName => skillTemplate.skillName;
    public SkillType SkillType => skillTemplate.skillType;
    public SkillElement Element => skillTemplate.skillElement;
    public string Description => skillTemplate.description;
    public int CurrentLevel => currentLevel;
    public bool IsMaxLevel => currentLevel == skillTemplate.maxLevel;

    public virtual void Setup(SkillTemplate skillTemplate, PlayerBase owner)
    {
        this.skillTemplate = skillTemplate;
        this.owner = owner;
    }

    public void TryLevelUp()
    {
        if( IsMaxLevel )
        {
            Logger.Log($"[{SkillName}] 스킬 최고 레벨 도달");
            return;
        }

        currentLevel++;

        OnLevelUp();

    }
    public abstract void OnLevelUp();       // 스킬 레벨업 시 1회 호출
    public abstract void OnSkill();         // 스킬 사용 시 호출
}
