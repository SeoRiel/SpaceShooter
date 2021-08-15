using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;                           // NavMeshAgent를 사용하기 위해 추가하는 namespace

[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{
    public List<Transform> wayPoints;           // 순찰 지점들을 저장하기 위한 List Type 변수
    public int nextIndex;                       // 순찰 지점을 저장할 Array Index의 길이

    private NavMeshAgent agent;                 // NavMeshAgent Component를 저장할 변수

    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f; 

    private bool _patrolling;                   // 순찰 여부를 판단하는 변수
    public bool patrolling                      // patrolling property 정의
    {
        get
        {
            return _patrolling;
        }

        set
        {
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = patrolSpeed;
                MoveWaypoint();
            }
        }
    }

    private Vector3 _traceTarget;                  // 추적 대상의 위치를 저장하는 변수
    public Vector3 traceTarget                     // traceTarget Property 정의
    {
        get
        {
            return _traceTarget;
        }

        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            TraceTarget(_traceTarget);
        }
    }

    public float speed
    {
        get
        {
            return agent.velocity.magnitude;
        }
    }

    private void Start()
    {
        // NavMeshAgent Component를 추출한 후 변수에 저장
        agent = GetComponent<NavMeshAgent>();
        // 목적지에 가까워질수록 속도가 감소하는 옵션 비활성화
        agent.autoBraking = false;

        // Hierarchy view의 wayPointGroup Game Obejct를 추출
        var group = GameObject.Find("WayPointGroup");
        if (group != null)
        {
            // wayPointGroup의 하위에 있는 모든 Transform Component를 추출한 뒤,
            // List type의 wayPoints array에 추가
            group.GetComponentsInChildren<Transform>(wayPoints);

            // 배열의 첫번째 항목 삭제
            wayPoints.RemoveAt(0);
        }

        MoveWaypoint();
    }

    // 다음 목적지까지 이동 명령을 내리는 함수
    private void MoveWaypoint()
    {
        // 최단거리 경로 계산이 끝나지 않으면 다음을 수행하지 않음
        if(agent.isPathStale)
        {
            return;
        }

        // 다음 목적지를 wayPoints Array에서 추출한 위치로 다음 목적지를 지정
        agent.destination = wayPoints[nextIndex].position;

        // 내비게이션 기능을 활성화해서 이동 시작
        agent.isStopped = false;
    }

    private void Update()
    {
        // 순찰 모드가 아닐 경우 이후 로직을 수행하지 않음
        if(!_patrolling)
        {
            return;
        }

        // NavMeshAgent가 이동하고 있고, 목적지 도착 여부 계산
        if(agent.velocity.sqrMagnitude >= 0.2f * 0.2f
            && agent.remainingDistance <= 5.0f)
        {
            // 다음 목적지의 배열 첨자를 계산
            nextIndex = ++nextIndex % wayPoints.Count;
            // 다음 목적지로 이동 명령 수행
            MoveWaypoint();
        }
    }

    // 다음 목적지까지 이동 명령을 내리는 함수
    private void TraceTarget(Vector3 position)
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
        // 바로 정지하기 위해 속도를 0으로 설정
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }
}
