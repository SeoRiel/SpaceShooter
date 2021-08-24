using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;                           // NavMeshAgent�� ����ϱ� ���� �߰��ϴ� namespace

[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{
    public List<Transform> wayPoints;           // ���� �������� �����ϱ� ���� List Type ����
    public int nextIndex;                       // ���� ������ ������ Array Index�� ����

    private NavMeshAgent agent;                 // NavMeshAgent Component�� ������ ����

    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;

    private float damping = 1.0f;               // ȸ���� �� �ӵ��� �����ϴ� ���
    private Transform enemyTransform;           // �� ĳ������ Transform Component�� ������ ����

    private bool _patrolling;                   // ���� ���θ� �Ǵ��ϴ� ����
    public bool patrolling                      // patrolling property ����
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
                damping = 1.0f;                    // ���� ������ ȸ�� ���
                MoveWaypoint();
            }
        }
    }

    private Vector3 _traceTarget;                  // ���� ����� ��ġ�� �����ϴ� ����
    public Vector3 traceTarget                     // traceTarget Property ����
    {
        get
        {
            return _traceTarget;
        }

        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            damping = 7.0f;                        // ���� ������ ȸ�� ���
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
        // �� ĳ������ Tansform Component�� ���� ��, ������ ����
        enemyTransform = GetComponent<Transform>();

        // NavMeshAgent Component�� ������ �� ������ ����
        agent = GetComponent<NavMeshAgent>();

        // �������� ����������� �ӵ��� �����ϴ� �ɼ� ��Ȱ��ȭ
        agent.autoBraking = false;

        // �ڵ����� ȸ���ϴ� ��� ��Ȱ��ȭ
        agent.updateRotation = false;

        // Hierarchy view�� wayPointGroup Game Obejct�� ����
        var group = GameObject.Find("WayPointGroup");
        if (group != null)
        {
            // wayPointGroup�� ������ �ִ� ��� Transform Component�� ������ ��,
            // List type�� wayPoints array�� �߰�
            group.GetComponentsInChildren<Transform>(wayPoints);

            // �迭�� ù��° �׸� ����
            wayPoints.RemoveAt(0);

            // ù��°�� �̵��� ��ġ�� �ұ�Ģ�ϰ� ����
            nextIndex = Random.Range(0, wayPoints.Count);
        }

        MoveWaypoint();
    }

    // ���� ���������� �̵� ����� ������ �Լ�
    private void MoveWaypoint()
    {
        // �ִܰŸ� ��� ����� ������ ������ ������ �������� ����
        if(agent.isPathStale)
        {
            return;
        }

        // ���� �������� wayPoints Array���� ������ ��ġ�� ���� �������� ����
        agent.destination = wayPoints[nextIndex].position;

        // ������̼� ����� Ȱ��ȭ�ؼ� �̵� ����
        agent.isStopped = false;
    }

    private void Update()
    {
        // �� ĳ���Ͱ� �̵� ���� ���� ȸ��
        if(agent.isStopped == false)
        {
            // NavMeshAgent�� ������ ���� ���͸� Quaternion Type�� ������ ��ȯ
            Quaternion rotate = Quaternion.LookRotation(agent.desiredVelocity);

            // ���� �Լ��� �̿��� ������ ȸ�� ���
            enemyTransform.rotation = Quaternion.Slerp(enemyTransform.rotation, rotate, Time.deltaTime * damping);
        }

        // ���� ��尡 �ƴ� ��� ���� ������ �������� ����
        if(!_patrolling)
        {
            return;
        }

        // NavMeshAgent�� �̵��ϰ� �ְ�, ������ ���� ���� ���
        if(agent.velocity.sqrMagnitude >= 0.04f
            && agent.remainingDistance <= 5.0f)
        {
            // ���� �������� �迭 ÷�ڸ� ���
            //nextIndex = ++nextIndex % wayPoints.Count;
            nextIndex = Random.Range(0, wayPoints.Count);

            // ���� �������� �̵� ��� ����
            MoveWaypoint();
        }
    }

    // ���� ���������� �̵� ����� ������ �Լ�
    private void TraceTarget(Vector3 position)
    {
        if(agent.isPathStale)
        {
            return;
        }

        agent.destination = position;
        agent.isStopped = false;
    }

    // ���� �� ������ ������Ű�� �Լ�
    public void Stop()
    {
        agent.isStopped = true;
        // �ٷ� �����ϱ� ���� �ӵ��� 0���� ����
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }
}
