using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // 적 캐릭터의 상태를 표현하기 위한 열거형 변수 정의
    public enum State
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }

    public State state = State.PATROL;  // 상태를 저장할 변수

    private Transform playerTransform;  // 플레이어 캐릭터의 위치를 저장할 변수
    private Transform enemyTransform;   // 적 캐릭터의 위치를 저장할 변수
    private Animator animator;          // Animator Component를 저장할 변수

    public float attackDistance = 5.0f; // 공격 사거리
    public float traceDistance = 10.0f; // 추적 사거리

    public bool isDie = false;          // 사망 여부를 판단하는 함수
    private WaitForSeconds wForS;       // 코루틴에서 사용할 지연 변수

    private MoveAgent moveAgent;        // 이동을 제어하는 MoveAgnet class를 저장할 변수
    private EnemyFire enemyFire;        // 총알 발사를 제어하는 EnemyFire class를 저장할 변수

    // 애니메이터 컨트롤러에 정의한 파라미터의 해시값을 미리 추출
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashDieIndex = Animator.StringToHash("DieIndex");
    private readonly int hashOffset = Animator.StringToHash("Offest");
    private readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");

    private void Awake()
    {
        // 주인공 게임 오브젝트 추출
        GameObject player = GameObject.FindGameObjectWithTag("PLAYER");

        // 플레이어 캐릭터를 발견할 경우
        if (player != null)
        {
            // 플레이어 캐릭터의 Transform Component 추출
            playerTransform = player.GetComponent<Transform>();
        }

        // 적 캐릭터의 Transform Component 추출
        enemyTransform = GetComponent<Transform>();

        // Animator Component 추출
        animator = GetComponent<Animator>();
        
        // 이동을 제어하는 MoveAgnet class를 저장하는 변수
        moveAgent = GetComponent<MoveAgent>();

        // 총알 발사를 제어하는 EnemyFire class 추출
        enemyFire = GetComponent<EnemyFire>();

        // 코루틴의 지연 시간 생성
        wForS = new WaitForSeconds(0.3f);

        // Cycle Offset 값을 불규칙하게 변경
        animator.SetFloat(hashOffset, Random.Range(0.0f, 1.0f));

        // Speed 값을 불규칙하게 변경
        animator.SetFloat(hashWalkSpeed, Random.Range(1.0f, 1.2f));
    }

    private void OnEnable()
    {
        // CheckState Coroutine 함수 실행
        StartCoroutine(CheckState());

        // Action Coroutine 함수 실행
        StartCoroutine(Action());

        Damage.OnPlayerDie += this.OnPlayerDie;
    }

    private void OnDisable()
    {
        Damage.OnPlayerDie += this.OnPlayerDie;
    }

    // 적 캐릭터의 상태를 검사하는 Coroutine Function
    private IEnumerator CheckState()
    {
        // 적 캐릭터가 사망하기 전까지 도는 무한루프
        while(!isDie)
        {
            // 상태가 사망이면 코루틴 함수 종료
            if(state == State.DIE)
            {
                yield break;
            }

            // 주인공과 적 캐릭터 간의 거리 계산
            float distance = Vector3.Distance(playerTransform.position, enemyTransform.position);
            // float distance = (playerTransform.position - enemyTransform.position).sqrMagnitude;

            // 공격 사정거리 이내인 경우
            if(distance <= attackDistance)
            {
                state = State.ATTACK;
            }
            // 추적 사정거리 이내인 경우
            else if( distance <= traceDistance)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }

            // 0.3초 동안 대기 상태에서 제어권 양도
            yield return wForS;
        }
    }

    // 상태에 따라 적 캐릭터의 행동을 처리하는 Coroutine Function
    private IEnumerator Action()
    {
        // 적 캐릭터가 사망할 때까지 무한루프
        while (!isDie)
        {
            yield return wForS;

            // 상태에 따라 분기 처리
            switch(state)
            {
                case State.PATROL:
                    // 총알 발사 정지
                    enemyFire.isFire = false;
                    // 순찰 모드를 활성화
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;

                case State.TRACE:
                    // 총알 발사 정지
                    enemyFire.isFire = false;
                    // 주인공의 위치를 넘겨 추적 모드로 진행
                    moveAgent.traceTarget = playerTransform.position;
                    animator.SetBool(hashMove, true);
                    break;

                case State.ATTACK:
                    // 순찰 및 추적 중지
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);

                    // 총알 발사 시작
                    if(enemyFire.isFire == false)
                    {
                        enemyFire.isFire = true;
                    }
                    break;

                case State.DIE:
                    this.gameObject.tag = "Untagged";
                    isDie = true;
                    enemyFire.isFire = false;
                    // 순찰 및 추적 중지
                    moveAgent.Stop();
                    // 사망 애니메이션의 종류를 지정
                    animator.SetInteger(hashDieIndex, Random.Range(0, 3)); 
                    // 사망 애니메이션 실행
                    animator.SetTrigger(hashDie);
                    // Capsule Collider Component를 비활성화
                    GetComponent<CapsuleCollider>().enabled = false;
                    break;
            }
        }
    }

    private void Update()
    {
        // Speed Parameter에 이동 속도 전달
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }

    public void OnPlayerDie()
    {
        moveAgent.Stop();
        enemyFire.isFire = false;

        // 모든 Coroutine 함수 종료
        StopAllCoroutines();
        animator.SetTrigger(hashPlayerDie);
    }

}