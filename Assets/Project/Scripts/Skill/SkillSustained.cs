using System.Collections.Generic;
using UnityEngine;

public class SkillSustained : SkillBase
{
    private float distanceToPlayer = 2f;
    private float prevDamage;       // 곡괭이 속성보너스 적용하기 위한 변수
    private Transform parent;
    private List<GameObject> pickaxs = new List<GameObject>();



    public override void Setup(SkillTemplate skillTemplate, PlayerBase owner, Transform spawnPoint = null)
    {
        base.Setup(skillTemplate, owner, spawnPoint);

        // 미리 제작한 곡괭이의 부모 오브젝트
        parent = GameObject.Find("Pickaxs").transform;

        prevDamage = 0f;
    }

    public override void OnLevelUp()
    {
        // 레벨이 0에서 1로 늘 때는 스탯 증가하지 않고 곡괭이 오브젝트만 생성
        if( currentLevel <= 1)
        {
            AddPickax((int)GetStat(StatType.ProjectileCount).Value);

            // 현재 활성화된 모든 곡괭이 위치 재설정
            int pickaxCount = parent.childCount;
            for(int i=0; i < pickaxCount; i++)
            {
                float angle = (360 / pickaxCount) * i;
                Vector3 position = Utils.GetPositionFromAngle(distanceToPlayer, angle);
                parent.GetChild(i).position = parent.position + position;
            }

            return;
        }

        // 공격 스킬 레벨업 시 공격력 등 스탯 갱신
        skillTemplate.attackBuffStats.ForEach(stat =>
        {
            GetStat(stat).BonusValue += stat.DefaultValue;
        });

    }

    private void AddPickax(int count)
    {
        float damage = CalculateDamage();
        prevDamage = damage;
        for(int i=0; i < count; i++)
        {
            GameObject clone = GameObject.Instantiate(skillTemplate.projectile, parent);
            clone.GetComponent<ProjectileCollision2D>().Setup(null, damage);
            pickaxs.Add(clone);
        }
    }
    // 곡괭이는 데미지가 생성 직후, 레벨업 후에 적용되는데 
    // 스킬의 레벨업이 속성 보너스 레벨업 보다 빠르기 때문에 지속스킬의 경우 속성 보너스 제대로 적용안되는
    // 현상이 있었다. 이를 해결하기 위해 OnSkill()을 활용해 SkillSystem에서 OnSkill()이 호출될 때 적용되도록 수정했다.
    private void UpdateDamage(float damage)
    {
        foreach (var item in pickaxs)
        {
            item.GetComponent<ProjectileCollision2D>().Setup(null, damage);
        }
    }

    public override void OnSkill()
    {
        float currentDamage = CalculateDamage();

        if (Mathf.Approximately(prevDamage, currentDamage))
        {
            return;
        }

        UpdateDamage(currentDamage);
        prevDamage = currentDamage;
    }

   

}