using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerData : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private Image fillGaugeEXP;
    [SerializeField]
    private PlayerBase entity;

    private void Awake()
    {
        entity.Stats.CurrentEXP.OnValueChanged += UpdateEXP;
    }
    private void UpdateEXP(Stat stat, float prev, float current)
    {
        textLevel.text = $"Lv.{entity.Stats.GetStat(StatType.Level).Value}";
        fillGaugeEXP.fillAmount = entity.Stats.CurrentEXP.Value / entity.Stats.GetStat(StatType.Experience).Value;
    }
}
