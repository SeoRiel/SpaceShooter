using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;                           // NavMeshAgent를 사용하기 위해 추가하는 namespace

[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{
    public List<Transform> wayPoints;           // 순찰 지점들을 저장하기 위한 List Type Parameter
    public int nextIndex;                       // 다음 순찰 지점의 Array Index
   
    private NavMeshAgent agent;                 // NavMeshAgent Component를 저장할 변수
    
    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;

    private float damping = 1.0f;               // 회전할 때의 속도를 조절하는 계수
    private Transform enemyTransform;           // 적 캐릭터의 Transform Component를 저장할 변수

    private bool bPatrolling;
   
    // patrolling property 정의
    public bool patrolling
    {
        get 
        {
            return bPatrolling;
        }
        
        set
        {
            bPatrolling = value;
            if(bPatrolling)
            {
                agent.speed = patrolSpeed;
                damping = 1.0f;                 // 순찰 상태의 회전 계수 
                MoveWayPoint();
            }
        }
    }

    // 추적 대상의 위치를 저장하는 변수
    private Vector3 _traceTarget;

    // traceTarget Property의 정의
    public Vector3 traceTarget
    {   
        // getter
        get
        {
            return _traceTarget;
        }

        // setter
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            damping = 7.0f;                     // 추적 상태의 회전 계수
            Tracetarget(_traceTarget);
        }
    }

    // NavMeshAgent의 이동 속도에 대한 Property 정의
    public float speed
    {
        // getter
        get
        {
            return agent.velocity.magnitude;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        // 적 캐릭터의 Transform Component 추출 후, 변수에 저장
        enemyTransform = GetComponent<Transform>();

        // NavMeshAgent Component를 추출한 후, 변수에 저장 
        agent = GetComponent<NavMeshAgent>();

        // 목적지에 가까워질수록 속도를 줄이는 옵션 비활성화
        agent.autoBraking = false;
        // 자동으로 회전하는 기능 비활성화
        agent.updateRotation = false;
        agent.speed = patrolSpeed;

        // 하이러키 뷰의 WayPointGroup GameObject를 추출
        var group = GameObject.Find("WayPointGroup");
        if( group != null)
        {
            // WayPointGroup 하위에 있는 모든 Transform Component를 추출한 뒤,
            // List Type의 wayPoints Array에 추가
            group.GetComponentsInChildren<Transform>(wayPoints);

            // 배열의 첫번째 항목 삭제
            wayPoints.RemoveAt(0);
        }

        MoveWayPoint();
    }

    // 다음 목적지까지 이동 명령을 내리는 함수
    private void MoveWayPoint()
    {
        // 최단거리 경로 계산이 끝나지 않았으면 다음 코드를 수행하지 않음
        if(agent.isPathStale)
        {
            return;
        }

        // 다음 목적지를 WayPoints 배열에서 추출한 위치로 다음 목적지 지정
        agent.destination = wayPoints[nextIndex].position;

        // 내비게이션 기능 활성화 후, 이동 시작
        agent.isStopped = false;
    }

    // 플레이어 캐릭터를 추적할 때 이동시키는 함수
    private void Tracetarget(Vector3 position)
    {
        if(agent.isPathStale)
        {
            return;
        }

        agent.destination = position;
        agent.isStopped = false;
    }

    // 순찰 및 추적을 정지시키는 함수
    public void Stop()
    {
        agent.isStopped = true;

        // 즉시 정지를 위한 이동속도 0으로 설정
        agent.velocity = Vector3.zero;
        patrolling = false;
    }

    private void Update()
    {
        // 적 캐릭터가 이동 중일 때만 회전
        if(agent.isStopped == false)
        {
            // NavMeshAgent가 가야할 방향 벡터를 Quaternion Type의 각도로 변경
            Quaternion rotate = Quaternion.LookRotation(agent.desiredVelocity);

            // 보간 함수를 사용해 점진적으로 회전
            enemyTransform.rotation = Quaternion.Slerp(enemyTransform.rotation, rotate, Time.deltaTime * damping);
        }

        // 순찰 모드가 아닌 경우에는 이후 로직을 실행하지 않음
        if(!bPatrolling)
        {
            return;
        }

        // NavMeshAgent가 이동 중인 상태 또는 목적지 도착 여부 계산
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f
            && agent.remainingDistance <= 0.5f)
        {
            // 다음 목적지의 배열 첨자 계산
            nextIndex = ++nextIndex % wayPoints.Count;

            // 다음 목적지로 이동 명령 수행
            MoveWayPoint();
        }
    }

}
