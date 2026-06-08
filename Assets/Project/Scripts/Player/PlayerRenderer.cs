using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{
    [SerializeField]
    private Transform playerModel;  // 좌우 반전을 위한 플레이어 Transform
    [SerializeField]
    private ParticleSystem footStepEffect;
    private ParticleSystem.EmissionModule footEmission;
    private Animator animator;

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
