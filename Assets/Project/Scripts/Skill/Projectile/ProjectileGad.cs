using UnityEngine;

public class ProjectileGad : MonoBehaviour
{
    [SerializeField]
    private Transform hitEffect;
    [SerializeField]
    private UIDamageText damageText;
    private MovementRigidbody2D moventRigidbody2D;
    private ScaleEffect scaleEffect;
    private EntityBase target;
    private float damage;
    private bool isCritical;

    public void Setup(EntityBase target, float damage, bool isCritical)
    {
        moventRigidbody2D = GetComponent<MovementRigidbody2D>();
        scaleEffect = GetComponent<ScaleEffect>();
        this.target = target;
        this.damage = damage;
        this.isCritical = isCritical;

        // 발사체 크기를 35%에서 100%로 확대
        scaleEffect.Play(transform.localScale * 0.35f, transform.localScale);
        // 발사체를 목표 방향으로 회전
        transform.rotation = Utils.RotateToTarget(transform.position, target.MiddlePoint, 90);
        // 발사체 이동 방향 설정
        moventRigidbody2D.MoveTo((target.MiddlePoint - transform.position).normalized);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.CompareTag("Wall") )
        {
            Destroy(gameObject);
        }
        else if( collision.CompareTag("Enemy") && 
           collision.TryGetComponent<EntityBase>(out var entity))
        {
            if ( entity != target )
                return;

            if(hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }
            if(damageText != null)
            {
                UIDamageText clone = Instantiate(damageText, transform.position, Quaternion.identity);
                clone.Setup(damage.ToString("F0"), isCritical ? Color.red : Color.white);
            }
            entity.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
