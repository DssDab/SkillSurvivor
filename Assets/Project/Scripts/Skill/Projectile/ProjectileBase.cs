using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    protected MovementRigidbody2D movement2D;

    public virtual void Setup(EntityBase target, float damage)
    {
        movement2D = GetComponent<MovementRigidbody2D>();

        // 발사체 크기를 20%에서 100%로 확대
        GetComponent<ScaleEffect>().Play(transform.localScale * 0.2f, transform.localScale);
        // 적 오브젝트와 충돌 처리
        GetComponent<ProjectileCollision2D>().Setup(target, damage);
    }

    public virtual void Setup(EntityBase target, float damage, int maxCount, int index)
    {
        Setup(target, damage);
    }

    private void Update()
    {
        Process();
    }
    public abstract void Process();
}
