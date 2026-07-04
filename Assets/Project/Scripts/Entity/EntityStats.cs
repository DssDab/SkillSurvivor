using System.Linq;
using UnityEngine;

[System.Serializable]
public struct EntityStats
{

    [Header("Current Stats")]
    [SerializeField]
    private Stat currentExp;
    [SerializeField]
    private Stat currentHP;

    [Header("Stats")]
    [SerializeField]
    private Stat[] stats;

    public readonly Stat CurrentEXP => currentExp;
    public readonly Stat CurrentHP => currentHP;
    public readonly Stat GetStat(Stat stat) =>
        stats.FirstOrDefault(s => s.StatType == stat.StatType);
    public readonly Stat GetStat(StatType statType) =>
        stats.FirstOrDefault(s => s.StatType == statType);
}
