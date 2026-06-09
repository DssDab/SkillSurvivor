using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private bool x, y, z;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void Update()
    {
        if (target == null) return;

        // 활성화 축은 target 위치, 비활성화 축은 자기 자신의 위치로 설정
        transform.position = new Vector3(
            (x ? target.transform.position.x : transform.position.x),
            (y ? target.transform.position.y : transform.position.y),
            (z ? target.transform.position.z : transform.position.z));
    }

}
