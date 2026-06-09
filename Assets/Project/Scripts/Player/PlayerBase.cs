using UnityEngine;
public class PlayerBase : EntityBase
{
    [SerializeField]
    private FollowTarget targetMask;

    // 현재 플레이어가 이동 중인지
    // 스킬 사용 등 여러 곳에서 필요하므로 public 속성으로 정의
    public bool IsMoved { get; set; } = false;

    private void Awake()
    {
        base.Setup();
    }
    private void Update()
    {
        if( Target == null ) targetMask.gameObject.SetActive( false );

        SearchTarget();
    }

    private void SearchTarget()
    {
        float closestDistSqr = Mathf.Infinity;

        foreach( var entity in EnemySpawner.Enemies)
        {
            // 가장 가까운 대상을 찾으므로 sqrMagnitude 사용
            float distance = (entity.transform.position - transform.position).sqrMagnitude;
            if(distance < closestDistSqr)
            {
                closestDistSqr = distance;
                Target = entity.GetComponent<EntityBase>();
            }
        }

        if( Target != null)
        {
            targetMask.SetTarget(Target.transform);
            targetMask.transform.position = Target.transform.position;
            targetMask.gameObject.SetActive (true);
        }
    }
}
