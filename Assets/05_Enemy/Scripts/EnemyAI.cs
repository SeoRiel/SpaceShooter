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
    private EnemyFire enemyFire;        // �Ѿ� �߻縦 �����ϴ� EnemyFire class�� ������ ����

    // �ִϸ����� ��Ʈ�ѷ��� ������ �Ķ������ �ؽð��� �̸� ����
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashDieIndex = Animator.StringToHash("DieIndex");
    private readonly int hashOffset = Animator.StringToHash("Offest");
    private readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");

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

        // �Ѿ� �߻縦 �����ϴ� EnemyFire class ����
        enemyFire = GetComponent<EnemyFire>();

        // �ڷ�ƾ�� ���� �ð� ����
        wForS = new WaitForSeconds(0.3f);

        // Cycle Offset ���� �ұ�Ģ�ϰ� ����
        animator.SetFloat(hashOffset, Random.Range(0.0f, 1.0f));

        // Speed ���� �ұ�Ģ�ϰ� ����
        animator.SetFloat(hashWalkSpeed, Random.Range(1.0f, 1.2f));
    }

    private void OnEnable()
    {
        // CheckState Coroutine �Լ� ����
        StartCoroutine(CheckState());

        // Action Coroutine �Լ� ����
        StartCoroutine(Action());

        Damage.OnPlayerDie += this.OnPlayerDie;
    }

    private void OnDisable()
    {
        Damage.OnPlayerDie += this.OnPlayerDie;
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
                    // �Ѿ� �߻� ����
                    enemyFire.isFire = false;
                    // ���� ��带 Ȱ��ȭ
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;

                case State.TRACE:
                    // �Ѿ� �߻� ����
                    enemyFire.isFire = false;
                    // ���ΰ��� ��ġ�� �Ѱ� ���� ���� ����
                    moveAgent.traceTarget = playerTransform.position;
                    animator.SetBool(hashMove, true);
                    break;

                case State.ATTACK:
                    // ���� �� ���� ����
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);

                    // �Ѿ� �߻� ����
                    if(enemyFire.isFire == false)
                    {
                        enemyFire.isFire = true;
                    }
                    break;

                case State.DIE:
                    this.gameObject.tag = "Untagged";
                    isDie = true;
                    enemyFire.isFire = false;
                    // ���� �� ���� ����
                    moveAgent.Stop();
                    // ��� �ִϸ��̼��� ������ ����
                    animator.SetInteger(hashDieIndex, Random.Range(0, 3)); 
                    // ��� �ִϸ��̼� ����
                    animator.SetTrigger(hashDie);
                    // Capsule Collider Component�� ��Ȱ��ȭ
                    GetComponent<CapsuleCollider>().enabled = false;
                    break;
            }
        }
    }

    private void Update()
    {
        // Speed Parameter�� �̵� �ӵ� ����
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }

    public void OnPlayerDie()
    {
        moveAgent.Stop();
        enemyFire.isFire = false;

        // ��� Coroutine �Լ� ����
        StopAllCoroutines();
        animator.SetTrigger(hashPlayerDie);
    }

}