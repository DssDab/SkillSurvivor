using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyTopBar : MonoBehaviour
{
    [SerializeField]
    private LevelData levelData;
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private Slider fillGaugeEXP;
    [SerializeField]
    private TextMeshProUGUI textHeart;
    [SerializeField]
    private TextMeshProUGUI textHeartTimer;
    [SerializeField]
    private TextMeshProUGUI textGEMCount;

    private void Awake()
    {
        // 현재는 로비에서 재화를 사용하지 않으므로 로비 씬을 로드할 때 1회만 호출
        // 일반적으로는 Stat.cs처럼 재화에 delegate, event 설정하고
        // 값이 변경될 때마다 호출하도록 설정해서 사용

        // 레벨, 경험치 계산과 출력
        UpdateLevel();
        // 보유 보석 출력
        textGEMCount.text = NotateNumber.Transform((long)Database.DBItem.goods.gem);
    }

    private void UpdateLevel()
    {
        int level = Database.DBItem.player.level;

        // 경험치가 최대이면 레벨업
        while(level < levelData.MaxLevel)
        {
            int expIndex = Mathf.Min(level - 1, levelData.MaxExperience.Length);
            float requiredExp = levelData.MaxExperience[expIndex];

            if (Database.DBItem.player.experience < requiredExp)
                break;


            Database.DBItem.player.experience -= requiredExp;

            level++;
            Database.DBItem.player.level = level;
        }
        Database.Write();

        fillGaugeEXP.value =
            Database.DBItem.player.experience / levelData.MaxExperience[level - 1];
        textLevel.text = Database.DBItem.player.level.ToString();
    }

    public void UpdateHeart(int current, int max)
    {
        textHeart.text = $"{current}/{max}";
    }

    public void UpdateHeartTimer(string text)
    {
        textHeartTimer.text = text;
    }
}
