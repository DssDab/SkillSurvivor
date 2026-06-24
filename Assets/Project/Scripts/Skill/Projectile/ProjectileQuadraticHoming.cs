using UnityEngine;

public class ProjectileQuadraticHoming : ProjectileBase
{
    private Vector2 start, end, point;
    private float percent = 0, duration = 1f, r = 5;
    private EntityBase target;

    public override void Setup(EntityBase target, float damage, int maxCount, int index)
    {
        base.Setup(target, damage);

        this.target = target;
        start = transform.position;
        end = target.MiddlePoint;

        float angle = 360 / maxCount * index;
        // 현재 플레이어의 회전값을 적용하고자 angle값에 더함
        angle += Utils.GetAngleFromPosition(start, end);
        point = Utils.GetNewPoint(start, angle, r);

    }
    public override void Process()
    {
        if (percent >= 1f)
            Destroy(gameObject);
        if (target != null)
            end = target.MiddlePoint;

        percent += Time.deltaTime / duration;
        transform.position = Utils.QuadraticCurve(start, point, end, percent);
    }
}
