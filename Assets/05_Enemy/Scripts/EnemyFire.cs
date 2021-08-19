using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    public bool isFire = false;                                         // 총알 발사 여부를 판단할 변수
    public AudioClip fireSFX;                                           // 주인공을 향해 회전할 속도 계수
    public AudioClip reloadSFX;                                         // 재장전 사운드를 저장할 변수
    public GameObject bulletObject;                                     // 적 캐릭터의 총알 프리팹
    public Transform firePosition;                                      // 총알의 발사 위치 정보
    public MeshRenderer muzzleFlash;                                    // MuzzleFlash의 MeshRenderer Component를 저장할 변수

    private AudioSource audio;                                          // AudioSource Component를 저장할 변수
    private Animator animator;                                          // Animator Component를 저장할 변수
    private Transform playerTransform;                                  // 플레이어 캐릭터의 Transform Component를 저장할 변수
    private Transform enemyTransform;                                   // 적 캐릭터의 Transform Component를 저장할 변수

    // Animator Controller에 정의한 파라미터의 Hash Value를 미리 추출
    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashReload = Animator.StringToHash("Reload");

    private float nextFire = 0.0f;                                      // 다음 발사할 시간 계산용 변수
    private readonly float fireRate = 0.1f;                             // 총알 발사 간격
    private readonly float damping = 10.0f;                             // 주인공을 향해 회전할 속도 계수

    private readonly float reloadTime = 2.0f;                           // 재장전 시간
    private readonly int maxBullet = 10;                                // 탄창의 최대 총알 개수
    private int currentBullet = 10;                                     // 초기 총알 개수
    private bool isReload = false;                                      // 재장전 여부
    private WaitForSeconds wsReload;                                    // 재장전 시간 동안 기다릴 변수 선언

    private void Start()
    {
        // Component 추출 및 저장
        playerTransform = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        enemyTransform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        wsReload = new WaitForSeconds(reloadTime);
    }

    private void Update()
    {
        if(!isReload && isFire)
        {
            // 현재 시간이 다음 발사 시간보다 큰지 확인
            if(Time.time >= nextFire)
            {
                Fire();
                // 다음 발사 시간 계산
                nextFire = Time.time + fireRate + Random.Range(0.0f, 0.3f);
            }

            // 주인공이 있는 위치까지 회전 각도 계산
            Quaternion rotate = Quaternion.LookRotation(playerTransform.position - enemyTransform.position);
            // 보간 함수를 이용한 점진적 회전
            enemyTransform.rotation = Quaternion.Slerp(enemyTransform.rotation, rotate, Time.deltaTime * damping);
        }
    }

    private void Fire()
    {
        animator.SetTrigger(hashFire);
        audio.PlayOneShot(fireSFX, 1.0f);

        // 총구 화염 효과 코루틴 호출
        StartCoroutine(ShowMuzzleFlash());

        // 총알 생성
        GameObject bullet = Instantiate(bulletObject, firePosition.position, firePosition.rotation);
        // 일정 시간이 지난 후, 삭제
        Destroy(bullet, 3.0f);

        // 남은 총알로 재장전 여부 계산
        isReload = (--currentBullet % maxBullet == 0);
        if(isReload)
        {
            StartCoroutine(Reloading());
        }
    }

    IEnumerator ShowMuzzleFlash()
    {
        // MuzzleFlash 활성화
        muzzleFlash.enabled = true;

        // 불규칙한 회전 각도 계산
        Quaternion rotate = Quaternion.Euler(Vector3.forward * Random.Range(0, 360));
        // MuzzleFlash를 Z축 방향으로 회전
        muzzleFlash.transform.localRotation = rotate;
        // MuzzleFlash의 스케일을 불규칙하게 조정
        muzzleFlash.transform.localScale = Vector3.one * Random.Range(1.0f, 2.0f);

        // 텍스처의 Offset 속성에 적용할 불규칙한 값을 생성
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;
        // MuzzleFlash의 Material의 Offset 값을 적용
        muzzleFlash.material.SetTextureOffset("_MainTex", offset);                      // _MainTex == Diffuse, _BumpMap == Normal Map, _Cube == CubeMap

        // MuzzleFlash가 보일 때동안 잠시 대기
        yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        // MuzzleFlash를 다시 비활성화
        muzzleFlash.enabled = false;
    }

    IEnumerator Reloading()
    {
        // MuzzleFlash를 비활성화
        muzzleFlash.enabled = false;

        // 재장전 애니메이션 실행
        animator.SetTrigger(hashReload);

        // 재장전 사운드 실행
        audio.PlayOneShot(reloadSFX, 1.0f);

        // 재장전 시간만큼 대기하는 동안 제어권 양도
        yield return wsReload;

        // 총알의 개수 초기화
        currentBullet = maxBullet;
        isReload = false;
    }


}
