using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{
    [SerializeField]
    private Transform playerModel;  // 좌우 반전을 위한 플레이어 Transform
    [SerializeField]
    private ParticleSystem footStepEffect;
    private ParticleSystem.EmissionModule footEmission;
    private Animator animator;
    [SerializeField]
    private Transform playerArmModel;

    public void LookRotation(PlayerBase playerBase)
    {
        if( playerBase.IsMoved)
        {
            playerArmModel.rotation = Quaternion.identity;
        }
        else
        {
            if (playerBase.Target == null) return;

            Vector3 target = playerBase.Target.MiddlePoint;
            // 목표물이 플레이어 왼쪽이면 -1, 오른쪽이면 1
            float flip = target.x - transform.position.x < 0 ? -1 : 1;
            // 플레이어 좌우반전
            SpriteFlipX(flip);
            // 플레이어 무기 회전
            // 왼쪽을 볼 때는 부모 오브젝트에 의해 회전이 적용되어
            // 무기 방향이 틀어지므로 180만큼 가중치를 줌
            playerArmModel.rotation = Utils.RotateToTarget(playerArmModel.position, target, (1 - flip) * 90);
        }
    }

    private void Awake()
    {
        footEmission = footStepEffect.emission;
        animator = GetComponent<Animator>();
    }

    public void OnMovement(float speed)
    {
        animator.SetFloat("moveSpeed", speed);
    }

    public void OnFootStepEffect(bool isMoved)
    {
        footEmission.rateOverTime = isMoved ? 20 : 0;
        if( footStepEffect.isPlaying == false ) footStepEffect.Play();
    }

    public void SpriteFlipX(float x)
    {
        Vector3 currentScale = playerModel.localScale;
        currentScale.x = x < 0 ? -1.5f : 1.5f;
        playerModel.localScale = currentScale;
    }
}
