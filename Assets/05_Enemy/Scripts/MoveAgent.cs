using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;                           // NavMeshAgent�� ����ϱ� ���� �߰��ϴ� namespace

[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{
    public List<Transform> wayPoints;           // ���� �������� �����ϱ� ���� List Type Parameter
    public int nextIndex;                       // ���� ���� ������ Array Index
   
    private NavMeshAgent agent;                 // NavMeshAgent Component�� ������ ����
    
    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;

    private float damping = 1.0f;               // ȸ���� ���� �ӵ��� �����ϴ� ���
    private Transform enemyTransform;           // �� ĳ������ Transform Component�� ������ ����

    private bool bPatrolling;
   
    // patrolling property ����
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
                damping = 1.0f;                 // ���� ������ ȸ�� ��� 
                MoveWayPoint();
            }
        }
    }

    // ���� ����� ��ġ�� �����ϴ� ����
    private Vector3 _traceTarget;

    // traceTarget Property�� ����
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
            damping = 7.0f;                     // ���� ������ ȸ�� ���
            Tracetarget(_traceTarget);
        }
    }

    // NavMeshAgent�� �̵� �ӵ��� ���� Property ����
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
        // �� ĳ������ Transform Component ���� ��, ������ ����
        enemyTransform = GetComponent<Transform>();

        // NavMeshAgent Component�� ������ ��, ������ ���� 
        agent = GetComponent<NavMeshAgent>();

        // �������� ����������� �ӵ��� ���̴� �ɼ� ��Ȱ��ȭ
        agent.autoBraking = false;
        // �ڵ����� ȸ���ϴ� ��� ��Ȱ��ȭ
        agent.updateRotation = false;
        agent.speed = patrolSpeed;

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

    // �÷��̾� ĳ���͸� ������ �� �̵���Ű�� �Լ�
    private void Tracetarget(Vector3 position)
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

        // ��� ������ ���� �̵��ӵ� 0���� ����
        agent.velocity = Vector3.zero;
        patrolling = false;
    }

    private void Update()
    {
        // �� ĳ���Ͱ� �̵� ���� ���� ȸ��
        if(agent.isStopped == false)
        {
            // NavMeshAgent�� ������ ���� ���͸� Quaternion Type�� ������ ����
            Quaternion rotate = Quaternion.LookRotation(agent.desiredVelocity);

            // ���� �Լ��� ����� ���������� ȸ��
            enemyTransform.rotation = Quaternion.Slerp(enemyTransform.rotation, rotate, Time.deltaTime * damping);
        }

        // ���� ��尡 �ƴ� ��쿡�� ���� ������ �������� ����
        if(!bPatrolling)
        {
            return;
        }

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
