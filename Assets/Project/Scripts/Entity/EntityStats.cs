using System.Linq;
using UnityEngine;

[System.Serializable]
public struct EntityStats
{
    [Header("Level, Exp")]
    public int level;       // 레벨
    public long exp;        // 경험치

    [Header("Current HP")]
    [SerializeField]
    private Stat currentHP;

    [Header("Stats")]
    [SerializeField]
    private Stat[] stats;

    public readonly Stat CurrentHP => currentHP;
    public readonly Stat GetStat(Stat stat) =>
        stats.FirstOrDefault(s => s.StatType == stat.StatType);
    public readonly Stat GetStat(StatType statType) =>
        stats.FirstOrDefault(s => s.StatType == statType);
}
