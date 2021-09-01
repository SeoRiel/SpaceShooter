using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    public GameObject hpBarPrefab;
    public Vector3 hpBarOffset = new Vector3(0.0f, 0.0f, 0.0f);

    private const string bulletTag = "BULLET";  
    private float hp = 100.0f;                  // 체력 게이지
    private float initHp = 100.0f;              // 초기 체력 게이지
    private GameObject bloodEffect;             // 피격 시 사용할 혈흔 효과

    private Canvas uiCanvas;                    // 부모가 될 Canvas 객체
    private Image hpbarImage;                   // 체력 게이지에 따라 fillAmount 속성을 변경할 Image

    // Start is called before the first frame update
    private void Start()
    {
        // EnemyAI 스크립트를 추출하여 변수에 저장
        // enemyAI = GetComponent<EnemyAI>();

        // 혈흔 효과 프리팹 로드
        bloodEffect = Resources.Load<GameObject>("BulletImpactFleshBigEffect");

        // 생명 게이지의 생성 및 초기화
        SetHpbar();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == bulletTag)
        {
            // 혈흔 효과를 생성하는 함수 호출
            ShowBloodEffect(collision);
            // 총알 삭제
            // Destroy(collision.gameObject);
            collision.gameObject.SetActive(false);

        }
        // 생명 게이지 차감
        hp -= collision.gameObject.GetComponent<BulletCtrl>().damage;
        // 생명 게이지의 fillAmount 속성을 변경
        hpbarImage.fillAmount = hp / initHp;
        
        if(hp <= 0.0f)
        {
            GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
            hpbarImage.GetComponentsInParent<Image>()[1].color = Color.clear;
        }
    }

    private void ShowBloodEffect(Collision collision)
    {
        // 총알이 충돌한 지점 산출
        Vector3 hitPosition = collision.contacts[0].point;

        // 총알이 충돌하여 발생한 법선 벡터
        Vector3 hitNormal = collision.contacts[0].normal;

        // 총알 충돌 시 발향 벡터의 회전값 계산
        Quaternion rotate = Quaternion.FromToRotation(-Vector3.forward, hitNormal);

        // 혈흔 효과 생성
        GameObject blood = Instantiate<GameObject>(bloodEffect, hitPosition, rotate);
        Destroy(blood, 1.0f);
    }

    private void SetHpbar()
    {
        uiCanvas = GameObject.Find("UI Canvas").GetComponent<Canvas>();
        // UI Canvas 하위로 HP bar 생성
        GameObject hpBar = Instantiate<GameObject>(hpBarPrefab, uiCanvas.transform);
        // fillAmount 속성을 변경할 Image를 추출
        hpbarImage = hpBar.GetComponentsInChildren<Image>()[1];

        // 생명 게이지가 따라가야할 대상과 오프셋 값 설정
        var chaseHpbar = hpBar.GetComponent<EnemyHpbar>();
        chaseHpbar.targetTransform = this.gameObject.transform;
        chaseHpbar.offset = hpBarOffset;
    }
}
