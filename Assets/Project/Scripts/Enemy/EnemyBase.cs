using UnityEngine;

public class EnemyBase : EntityBase
{
    [SerializeField]
    private Transform hudPoint;
    [SerializeField]
    private GameObject uiPrefab;
    private void Awake()
    {
        Setup();
    }

    protected override void Setup()
    {
        stats.maxHP = 100 + 50 * (stats.level - 1); // 기본(100) + 추가(50*(레벨-1))

        base.Setup();
    }

    public void Initialize(Transform parent)
    {
        GameObject clone = Instantiate(uiPrefab, parent);
        clone.transform.localScale = Vector3.one;
        clone.GetComponent<FollowTargetUI>().Setup(hudPoint);
        clone.GetComponent<UIHP>().Setup(this);
    }
}
