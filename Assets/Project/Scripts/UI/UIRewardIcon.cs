using TMPro;
using UnityEngine;

public class UIRewardIcon : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textReward;

    public void SetReward(long reward)
    {
        textReward.text = reward.ToString();
    }
}
