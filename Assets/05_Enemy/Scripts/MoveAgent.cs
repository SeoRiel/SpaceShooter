using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;                       // NavMeshAgent�� ����ϱ� ���� �߰��ϴ� namespace

[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{
    public List<Transform> wayPoints;       // ���� �������� �����ϱ� ���� List Type Parameter
    public int nextIndex;                   // ���� ���� ������ Array Index

    private NavMeshAgent agent;             // NavMeshAgent Component�� ������ ����

    // Start is called before the first frame update
    private void Start()
    {
        // NavMeshAgent Component�� ������ ��, ������ ���� 
        agent = GetComponent<NavMeshAgent>();

        // �������� ����������� �ӵ��� ���̴� �ɼ� ��Ȱ��ȭ
        agent.autoBraking = false;

        // ���̷�Ű ���� WayPointGroup GameObject�� ����
        var group = GameObject.Find("WayPointGroup");
        if( group != null)
        {
            // WayPointGroup ������ �ִ� ��� Transform Component�� ������ ��,
            // List Type�� wayPoints Array�� �߰�
            group.GetComponentsInChildren<Transform>(wayPoints);

            // �迭�� ù��° �׸� ����
            wayPoints.RemoveAt(0);
        }

        MoveWayPoint();
    }

    // ���� ���������� �̵� ����� ������ �Լ�
    private void MoveWayPoint()
    {
        // �ִܰŸ� ��� ����� ������ �ʾ����� ���� �ڵ带 �������� ����
        if(agent.isPathStale)
        {
            return;
        }

        // ���� �������� WayPoints �迭���� ������ ��ġ�� ���� ������ ����
        agent.destination = wayPoints[nextIndex].position;

        // ������̼� ��� Ȱ��ȭ ��, �̵� ����
        agent.isStopped = false;
    }

    private void Update()
    {
        // NavMeshAgent�� �̵� ���� ���� �Ǵ� ������ ���� ���� ���
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f
            && agent.remainingDistance <= 0.5f)
        {
            // ���� �������� �迭 ÷�� ���
            nextIndex = ++nextIndex % wayPoints.Count;

            // ���� �������� �̵� ��� ����
            MoveWayPoint();
        }
    }

}
