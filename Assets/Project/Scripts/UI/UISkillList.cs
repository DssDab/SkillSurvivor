using System.Collections.Generic;
using UnityEngine;

public class UISkillList : MonoBehaviour
{
    [SerializeField]
    private UISkillIcon skillIconPrefab;
    [SerializeField]
    private Transform[] skillElementType;
    [SerializeField]
    private Transform skillElementBonus;

    private Dictionary<string, UISkillIcon> skillIcons;

    public void Setup(Dictionary<string, SkillTemplate> skills, Dictionary<string, SkillTemplate> elementalBonus)
    {
        skillIcons = new Dictionary<string, UISkillIcon>();

        foreach(var item in skills)
        {
            SpawnIcon(item.Value, skillElementType[(int)item.Value.skillElement - 100]);
        }
        foreach(var item in elementalBonus)
        {
            SpawnIcon(item.Value, skillElementBonus);
        }
    }

    public void LevelUp(SkillBase skill)
    {
        if(skillIcons.ContainsKey(skill.SkillName))
        {
            skillIcons[skill.SkillName].LevelUp(skill.CurrentLevel, skill.EnableIcon);
        }
    }

    private void SpawnIcon(SkillTemplate skill, Transform parent)
    {
        var clone = Instantiate(skillIconPrefab, parent);
        clone.transform.localScale = Vector3.one;
        clone.Setup(skill.disableIcon);

        skillIcons.Add(skill.skillName, clone);
    }
}
