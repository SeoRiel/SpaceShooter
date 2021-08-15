using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // �� ĳ������ ���¸� ǥ���ϱ� ���� ������ ���� ����
    public enum State
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }
    public State state = State.PATROL;  // ���¸� ������ ����

    private Transform playerTransform;  // �÷��̾� ĳ������ ��ġ�� ������ ����
    private Transform enemyTransform;   // �� ĳ������ ��ġ�� ������ ����
    private Animator animator;          // Animator Component�� ������ ����

    public float attackDistance = 5.0f; // ���� ��Ÿ�
    public float traceDistance = 10.0f; // ���� ��Ÿ�

    public bool isDie = false;          // ��� ���θ� �Ǵ��ϴ� �Լ�
    private WaitForSeconds wForS;       // �ڷ�ƾ���� ����� ���� ����

    private MoveAgent moveAgent;        // �̵��� �����ϴ� MoveAgnet class�� ������ ����

    // �ִϸ����� ��Ʈ�ѷ��� ������ �Ķ������ �ؽð��� �̸� ����
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");

    private void Awake()
    {
        // ���ΰ� ���� ������Ʈ ����
        GameObject player = GameObject.FindGameObjectWithTag("PLAYER");

        // �÷��̾� ĳ���͸� �߰��� ���
        if (player != null)
        {
            // �÷��̾� ĳ������ Transform Component ����
            playerTransform = player.GetComponent<Transform>();
        }

        // �� ĳ������ Transform Component ����
        enemyTransform = GetComponent<Transform>();

        // Animator Component ����
        animator = GetComponent<Animator>();
        
        // �̵��� �����ϴ� MoveAgnet class�� �����ϴ� ����
        moveAgent = GetComponent<MoveAgent>();
        
        // �ڷ�ƾ�� ���� �ð� ����
        wForS = new WaitForSeconds(0.3f);
    }

    private void OnEnable()
    {
        // CheckState Coroutine �Լ� ����
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }

    // �� ĳ������ ���¸� �˻��ϴ� Coroutine Function
    private IEnumerator CheckState()
    {
        // �� ĳ���Ͱ� ����ϱ� ������ ���� ���ѷ���
        while(!isDie)
        {
            // ���°� ����̸� �ڷ�ƾ �Լ� ����
            if(state == State.DIE)
            {
                yield break;
            }

            // ���ΰ��� �� ĳ���� ���� �Ÿ� ���
            float distance = Vector3.Distance(playerTransform.position, enemyTransform.position);
            // float distance = (playerTransform.position - enemyTransform.position).sqrMagnitude;

            // ���� �����Ÿ� �̳��� ���
            if(distance <= attackDistance)
            {
                state = State.ATTACK;
            }
            // ���� �����Ÿ� �̳��� ���
            else if( distance <= traceDistance)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }

            // 0.3�� ���� ��� ���¿��� ����� �絵
            yield return wForS;
        }
    }

    // ���¿� ���� �� ĳ������ �ൿ�� ó���ϴ� Coroutine Function
    private IEnumerator Action()
    {
        // �� ĳ���Ͱ� ����� ������ ���ѷ���
        while (!isDie)
        {
            yield return wForS;

            // ���¿� ���� �б� ó��
            switch(state)
            {
                case State.PATROL:
                    // ���� ��带 Ȱ��ȭ
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;

                case State.TRACE:
                    // ���ΰ��� ��ġ�� �Ѱ� ���� ���� ����
                    moveAgent.traceTarget = playerTransform.position;
                    animator.SetBool(hashMove, true);
                    break;

                case State.ATTACK:
                    // ���� �� ���� ����
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    break;

                case State.DIE:
                    // ���� �� ���� ����
                    moveAgent.Stop();
                    break;
            }
        }
    }

    private void Update()
    {
        // Speed Parameter�� �̵� �ӵ� ����
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }
}