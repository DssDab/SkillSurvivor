using UnityEngine;
public class PlayerBase : EntityBase
{
    // 현재 플레이어가 이동 중인지
    // 스킬 사용 등 여러 곳에서 필요하므로 public 속성으로 정의
    public bool IsMoved { get; set; } = false;

    private void Awake()
    {
        base.Setup();
    }
}
