using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State                      // ������ ������ �̿��� �� ĳ������ ���¸� ����
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }
    public State state = State.PATROL;     // ���¸� ������ ����

    public float attackDistance = 5.0f;    // ���� ��Ÿ�
    public float traceDistance = 10.0f;    // ���� ��Ÿ�
    public bool bDie = false;              // ��� ���� �Ǵ�

    private Transform playerTransform;     // �÷��̾� ĳ������ ��ġ�� ������ ����
    private Transform enemyTransform;      // �� ĳ������ ��ġ�� ������ ����
    private WaitForSeconds waitForSeconds; // �ڷ�ƾ���� ����� ���� �ð� ����
    private MoveAgent moveAgent;           // �̵��� �����ϴ� MoveAgnet class�� ������ ����
    private Animator animator;             // Animator Component�� ������ ����

    // �ִϸ����� ��Ʈ�ѷ��� ������ �Ķ������ �ؽð��� ����
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");

    private float damping = 1.0f;          // ȸ���� ���� �ӵ��� �����ϴ� ���

    private void Awake()
    {
        // �÷��̾� ĳ���� ���� ������Ʈ ����
        var player = GameObject.FindGameObjectsWithTag("PLAYER");

        // �÷��̾� ĳ������ Transform Component ����
        if (player != null)
        {
            playerTransform = GetComponent<Transform>();
        }

        // �� ĳ������ Transform Component ����
        enemyTransform = GetComponent<Transform>();

        // Animator Component ����
        animator = GetComponent<Animator>();

        // �̵��� �����ϴ� MoveAgnet class�� ����
        moveAgent = GetComponent<MoveAgent>();

        // �ڷ�ƾ �Լ��� ���� �ð� ����
        waitForSeconds = new WaitForSeconds(0.3f);
    }

    // Update is called once per frame
    private void OnEnable()
    {
        // CheckState Coroutine �Լ� ����
        StartCoroutine(CheckState());

        // Action Coroutine �Լ� ����
        StartCoroutine(Action());
    }

    // �� ĳ������ ���¸� üũ�ϴ� Coroutine �Լ�
    private IEnumerator CheckState()
    {
        // �� ĳ���Ͱ� ����ϱ� ������ �����ϴ� ���� ����
        while(!bDie)
        {
            // ��� ������ ��, Coroutine �Լ� ����
            if(state == State.DIE)
            {
                yield break;
            }

            // �÷��̾�� �� ĳ���� ���� �Ÿ� ���
            float distance = Vector3.Distance(playerTransform.position, enemyTransform.position);

            // ���� �����Ÿ� �̳��� ���
            if(distance <= attackDistance)
            {
                state = State.ATTACK;
            }
            // ���� �����Ÿ� �̳��� ���
            else if(distance <= traceDistance)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }

            // 0.3�� ���� ��� ���°� �Ǹ�, ����� �纸
            yield return waitForSeconds;
        }
    }

    // ���¿� ���� �� ĳ������ �ൿ�� ó���ϴ� Coroutine �Լ�
    private IEnumerator Action()
    {
        // �� ĳ���Ͱ� ����� ������ ���� �ݺ�
        while(!bDie)
        {
            yield return waitForSeconds;

            // ���¿� ���� �б� ó��
            switch(state)
            {
                case State.PATROL:
                    // ���� ��� Ȱ��ȭ
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;

                case State.TRACE:
                    // ���ΰ��� ��ġ�� �Ѱ� ���� ��� Ȱ��ȭ
                    moveAgent.traceTarget = playerTransform.position;
                    animator.SetBool(hashMove, true);
                    break;

                case State.ATTACK:
                    // ���� �� ���� �ߴ�
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    break;

                case State.DIE:
                    // ���� �� ���� �ߴ�
                    moveAgent.Stop();
                    break;
            }
        }
    }

    private void Update()
    {
        // Speed Parameter�� �̵� �ӵ��� ����
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }
}
