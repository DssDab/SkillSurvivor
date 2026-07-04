using UnityEngine;

[System.Serializable]
public class Stat 
{
    public delegate void ValueChangedHandler(Stat stat, float prev, float current);
    public event ValueChangedHandler OnValueChanged;
    public event ValueChangedHandler OnValueMax;
    public event ValueChangedHandler OnValueMin;

    [SerializeField]
    private StatType statType;              // 공격력, 체력 등 스탯 속성
    [SerializeField]
    private float maxValue;                 // 최댓값
    [SerializeField]
    private float minValue;                 // 최솟값
    [SerializeField]
    private float defaultValue;             // 기본값
    [SerializeField]
    private float bonusValue;               // 아이템, 스킬 등으로 증가하는 추갓값

    public void CopyData(Stat newStat)
    {
        statType = newStat.statType;
        maxValue = newStat.MaxValue;
        minValue = newStat.MinValue;
        defaultValue = newStat.DefaultValue;
        bonusValue = newStat.BonusValue;
    }

    public StatType StatType => statType;
    public float MaxValue => maxValue;  
    public float MinValue => minValue;
    public float Value => Mathf.Clamp(defaultValue + bonusValue, minValue, maxValue);

    public float DefaultValue
    {
        get => defaultValue; 

        set
        {
            float prev = Value;
            defaultValue = Mathf.Clamp(value, minValue, maxValue);
            TryInvokeValueChangedEvent(prev, Value);
        }
    }
    public float BonusValue
    {
        get => bonusValue;
        set => bonusValue = value;
    }

    private void TryInvokeValueChangedEvent(float prev, float current)
    {
        if( !Mathf.Approximately(prev, current) )
        {
            OnValueChanged?.Invoke(this, prev, current);

            if (Mathf.Approximately(current, maxValue))
                OnValueMax?.Invoke(this, prev, maxValue);
            else if(Mathf.Approximately(current, minValue))
                OnValueMin?.Invoke(this, prev, minValue);
        }
    }
}
public enum StatType { Damage=0, CooldownTime, CriticalChance, CriticalMultiplier, HP, Evasion, MetastasisCount, HPRecovery, ProjectileCount, Duration, AttackRate,
Level, Experience,
IceElementalBonus = 100, FireElementalBonus, WindElementalBonus, LightElementalBonus, DarkElementalBonus}
