using UnityEngine;

[System.Serializable]
public struct EntityStats
{
    [Header("Level, Exp")]
    public int level;       // 레벨
    public long exp;        // 경험치

    [Header("Attack")]
    public float damage;                // 공격력
    public float cooldownTime;          // 기본 공격 쿨타임
    public float criticalChance;        // 크리티컬 확률
    public float criticalMultiplier;    // 크리티컬 공격력

    [Header("Defense")]
    public float currentHP;     // 현재 체력
    public float maxHP;         // 최대 체력
    public float evasion;       // 회피율
}
