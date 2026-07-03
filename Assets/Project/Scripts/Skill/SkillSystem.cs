using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    [SerializeField]
    private SkillGad skillGad;
    [SerializeField]
    private Transform skillSpawnPoint;
    [SerializeField]
    private UISkillList uiSkillList;
    [SerializeField]
    private UISelectSkill uiSelectSkill;
    [SerializeField]
    private GameController gameController;

    private PlayerBase owner;

    private Dictionary<string, SkillBase> skills = new Dictionary<string, SkillBase>();
    private Dictionary<SkillElement, int> elementalCounts = new Dictionary<SkillElement, int>();
    private Dictionary<SkillElement, SkillBase> elementalSkills = new Dictionary<SkillElement, SkillBase>();
    private void Awake()
    {
        owner = GetComponent<PlayerBase>();
        skillGad.Setup(owner, skillSpawnPoint);

        // Resources/Skills/ 폴더에 있는 모든 스킬 정보를 불러와
        // 스킬 생성 <파일 이름, SkillTemplate>
        var skillDict = Resources.LoadAll<SkillTemplate>("Skills/").ToDictionary(item => item.name, item => item);
        foreach(var item in skillDict)
        {
            SkillBase skill = null;
            if (item.Value.skillType.Equals(SkillType.Buff))
                skill = new SkillBuff();
            else if (item.Value.skillType.Equals(SkillType.Emission))
                skill = new SkillEmission();
            else if (item.Value.skillType.Equals(SkillType.Sustained))
                skill = new SkillSustained();
            else if (item.Value.skillType.Equals(SkillType.Global))
                skill = new SkillGlobal();

                skill.Setup(item.Value, owner, skillSpawnPoint);
            skills.Add(item.Key, skill);
            // 습득한 모든 스킬의 이름, 레벨, 설명 출력 
            Logger.Log($"[{skill.SkillName}]" +
                $"Lv. {skill.CurrentLevel}");
        }

        // 속성 보너스 스킬 등록
        var eSkillDict = Resources.LoadAll<SkillTemplate>("ElementalSkills/").ToDictionary(item => item.skillName, item => item);
        foreach(var item in eSkillDict)
        {
            SkillBase skill = new SkillBuff();
            skill.Setup(item.Value, owner, skillSpawnPoint);

            elementalCounts.Add(item.Value.skillElement, 0);        // 속성 스킬 레벨 카운트
            elementalSkills.Add(item.Value.skillElement, skill);    // 보너스 스킬 SkillBase

            Logger.Log($"{item.Value.skillElement}, {item.Value.skillName}");
        }

        uiSkillList.Setup(skillDict, eSkillDict);
    }

    private void Update()
    {
        // 레벨업 가능한 임의 스킬 3개를 선택하고 그 중 하나 레벨업 [Debug Test]
        if (UnityEngine.InputSystem.Keyboard.current.digit1Key.wasPressedThisFrame)
            StartSelectSkill();

        // 모든 공격 스킬 업데이트
        foreach(var item in skills)
        {
            if (item.Value.CurrentLevel == 0)
                continue;

            item.Value.OnSkill();
        }
        // 플레이어의 목표가 없거나 이동 중 이면 모든 스킬 사용 불가
        if (owner.Target == null || owner.IsMoved == true)
            return;

        // 모든 공격 스킬 쿨타임 업데이트
        foreach(var item in skills)
        {
            item.Value.IsSkillAvailable();
        }

        // 기본 공격 스킬 업데이트 
        skillGad.OnSkill();
    }

    public void LevelUp(SkillBase skill)
    {
        if(skills.ContainsValue(skill))
        {
            skill.TryLevelUp();
            uiSkillList.LevelUp(skill);
            Logger.Log($"Level Up [{skill.SkillName}] {skill.Element}, Lv. {skill.CurrentLevel}");

            // 해당 스킬이 소속된 속성의 총 스킬 레벨 합+1
            elementalCounts[skill.Element]++;
            // 해당 스킬의 속성 보너스 레벨 증가 여부 판단
            if (elementalCounts[skill.Element] % 3 == 0)
            {
                elementalSkills[skill.Element].TryLevelUp();
                uiSkillList.LevelUp(elementalSkills[skill.Element]);
                Logger.Log($"{skill.Element} Lv.{elementalSkills[skill.Element].CurrentLevel}");
            }
        }
    }

    public void StartSelectSkill()
    {
        // 습득 또는 레벨업 할 수 있는 임의의 3개 스킬 선택
        var randomSkills = GetRandomSkills(skills, 3);
        if(randomSkills == null)
        {
            Logger.Log("더 이상 습득할 수 있는 스킬이 없습니다.");
            return;
        }

        // 스킬 선택 중에는 일시 정지
        gameController.SetTimeScale(0);

        // 획득 가능한 3개의 스킬 정보를 UI에 출력
        uiSelectSkill.StartSelectSkillUI(this, randomSkills.ToArray());
    }

    public void EndSelectSkill(SkillBase skill)
    {
        LevelUp(skill);                     // {skill} 스킬 레벨업
        uiSelectSkill.EndSelectSkillUI();   // 스킬 선택 UI 비활성화
        gameController.SetTimeScale(1);     // 게임 다시 시작
    }

    private List<SkillBase> GetRandomSkills(Dictionary<string, SkillBase> skills, int count=3)
    {
        // 습득할 수 있는 스킬 목록
        var values = new List<SkillBase>(skills.Values.Where(skill => !skill.IsMaxLevel)).ToList();
        var randomSkills = new List<SkillBase>();

        count = values.Count == 0 ? 0 : count;

        if (count == 0) return null;

        for (int i=0; i< count; i++)
        {
            int index = Random.Range(0, values.Count);
            // index번째 임의의 항목 선택
            randomSkills.Add(values[index]);
            // 중복 방지를 제거하고자 선택한 항목 삭제
            values.RemoveAt(index);
        }

        Logger.Log($"선택 가능한 3개의 스킬\n{randomSkills[0].SkillName}, "
            + $"{randomSkills[1].SkillName}, " + $"{randomSkills[2].SkillName}");

        return randomSkills;
    }

}
