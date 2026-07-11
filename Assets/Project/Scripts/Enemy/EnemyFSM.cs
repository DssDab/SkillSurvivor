using System.Linq;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;


public class EnemyFSM : MonoBehaviour
{
    private EnemyBase owner;
    private NavMeshAgent navMeshAgent;        // 적 이동 경로 설정과 이동 제어
    private BehaviorGraphAgent behaviorAgent; // 적 행동 제어
    private WeaponBase currentWeapon;         // 현재 활성화된 무기
    private void Awake()
    {
        owner = GetComponent<EnemyBase>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
        currentWeapon = GetComponent<WeaponBase>();

        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        currentWeapon.Setup(owner);
    }

    public void Setup(EntityBase target, GameObject[] wayPoints)
    {
        owner.Target = target;

        behaviorAgent.SetVariableValue("PatrolPoints", wayPoints.ToList());
        behaviorAgent.SetVariableValue("Target", target.gameObject);
    }

}
