using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State                      // 열거형 변수를 이용한 적 캐릭터의 상태를 정의
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }
    public State state = State.PATROL;     // 상태를 저장할 변수

    public float attackDistance = 5.0f;    // 공격 사거리
    public float traceDistance = 10.0f;    // 추적 사거리
    public bool bDie = false;              // 사망 여부 판단

    private Transform playerTransform;     // 플레이어 캐릭터의 위치를 저장할 변수
    private Transform enemyTransform;      // 적 캐릭터의 위치를 저장할 변수
    private WaitForSeconds waitForSeconds; // 코루틴에서 사용할 지연 시간 변수
    private MoveAgent moveAgent;           // 이동을 제어하는 MoveAgnet class를 저장할 변수
    private Animator animator;             // Animator Component를 저장할 변수

    // 애니메이터 컨트롤러에 정의한 파라미터의 해시값을 추출
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");

    private float damping = 1.0f;          // 회전할 때의 속도를 조절하는 계수

    private void Awake()
    {
        // 플레이어 캐릭터 게임 오브젝트 추출
        var player = GameObject.FindGameObjectsWithTag("PLAYER");

        // 플레이어 캐릭터의 Transform Component 추출
        if (player != null)
        {
            playerTransform = GetComponent<Transform>();
        }

        // 적 캐릭터의 Transform Component 추출
        enemyTransform = GetComponent<Transform>();

        // Animator Component 추출
        animator = GetComponent<Animator>();

        // 이동을 제어하는 MoveAgnet class를 추출
        moveAgent = GetComponent<MoveAgent>();

        // 코루틴 함수의 지연 시간 설정
        waitForSeconds = new WaitForSeconds(0.3f);
    }

    // Update is called once per frame
    private void OnEnable()
    {
        // CheckState Coroutine 함수 실행
        StartCoroutine(CheckState());

        // Action Coroutine 함수 실행
        StartCoroutine(Action());
    }

    // 적 캐릭터의 상태를 체크하는 Coroutine 함수
    private IEnumerator CheckState()
    {
        // 적 캐릭터가 사망하기 전까지 실행하는 무한 루프
        while(!bDie)
        {
            // 사망 상태일 때, Coroutine 함수 종료
            if(state == State.DIE)
            {
                yield break;
            }

            // 플레이어와 적 캐릭터 간의 거리 계산
            float distance = Vector3.Distance(playerTransform.position, enemyTransform.position);

            // 공격 사정거리 이내인 경우
            if(distance <= attackDistance)
            {
                state = State.ATTACK;
            }
            // 추적 사정거리 이내인 경우
            else if(distance <= traceDistance)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }

            // 0.3초 동안 대기 상태가 되며, 제어권 양보
            yield return waitForSeconds;
        }
    }

    // 상태에 따라 적 캐릭터의 행동을 처리하는 Coroutine 함수
    private IEnumerator Action()
    {
        // 적 캐릭터가 사망할 때까지 무한 반복
        while(!bDie)
        {
            yield return waitForSeconds;

            // 상태에 따른 분기 처리
            switch(state)
            {
                case State.PATROL:
                    // 순찰 모드 활성화
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;

                case State.TRACE:
                    // 주인공의 위치를 넘겨 추적 모드 활성화
                    moveAgent.traceTarget = playerTransform.position;
                    animator.SetBool(hashMove, true);
                    break;

                case State.ATTACK:
                    // 순찰 및 추적 중단
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    break;

                case State.DIE:
                    // 순찰 및 추적 중단
                    moveAgent.Stop();
                    break;
            }
        }
    }

    private void Update()
    {
        // Speed Parameter에 이동 속도를 전달
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }
}
