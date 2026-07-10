using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyState { None = -1, Attack, }
public class EnemyFSM : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform projectileSpawnPoint;
    [SerializeField]
    private NavMeshAgent navMeshAgent;  // 적 이동 경로 설정과 이동 제어

    private EnemyBase owner;
    private EnemyState enemyState;

    private void Awake()
    {
        owner = GetComponent<EnemyBase>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        ChangeState(EnemyState.Attack);
    }

    public void Setup(EntityBase target)
    {
        owner.Target = target;

        // [Debuug Test] 게임이 시작된 순간의 플레이어 위치로 이동
        navMeshAgent.SetDestination(target.MiddlePoint);
    }

    public void ChangeState(EnemyState newState)
    {
        // 열거형 변수.ToString()은 열거형으로 정의한 변수 이름을 문자열로 반환한다.
        // 이를 이용해 열거형 이름과 코루틴 이름을 일치시켜
        // 열거형 변수에 따라 코루틴 함수 재생을 제어할 수 있다.

        // 이전에 재생 중이던 상태 종료
        StopCoroutine(enemyState.ToString());
        // 상태 변경
        enemyState = newState;
        // 새로운 상태 재생
        StartCoroutine(enemyState.ToString());
    }
    private IEnumerator Attack()
    {
        var wait = new WaitForSeconds(owner.Stats.GetStat(StatType.CooldownTime).Value);

        while ( true )
        {
            yield return wait;

            Vector3 target = owner.Target.MiddlePoint;
            GameObject clone = Instantiate(projectilePrefab);
            clone.transform.position = projectileSpawnPoint.position;
            clone.GetComponent<EnemyProjectile>().Setup(target, owner.Stats.GetStat(StatType.Damage).Value);
        }
    }
}
