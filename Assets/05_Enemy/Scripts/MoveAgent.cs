using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;                       // NavMeshAgent를 사용하기 위해 추가하는 namespace

[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{
    public List<Transform> wayPoints;       // 순찰 지점들을 저장하기 위한 List Type Parameter
    public int nextIndex;                   // 다음 순찰 지점의 Array Index

    private NavMeshAgent agent;             // NavMeshAgent Component를 저장할 변수

    // Start is called before the first frame update
    private void Start()
    {
        // NavMeshAgent Component를 추출한 후, 변수에 저장 
        agent = GetComponent<NavMeshAgent>();

        // 목적지에 가까워질수록 속도를 줄이는 옵션 비활성화
        agent.autoBraking = false;

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

    private void Update()
    {
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
