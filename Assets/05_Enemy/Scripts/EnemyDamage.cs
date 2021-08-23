using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private const string bulletTag = "BULLET";  
    private float hp = 100.0f;                  // 체력 게이지
    private GameObject bloodEffect;             // 피격 시 사용할 혈흔 효과

    // Start is called before the first frame update
    private void Start()
    {
        // 혈흔 효과 프리팹 로드
        bloodEffect = Resources.Load<GameObject>("BulletImpactFleshBigEffect");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == bulletTag)
        {
            // 혈흔 효과를 생성하는 함수 호출
            ShowBloodEffect(collision);
            // 총알 삭제
            Destroy(collision.gameObject);
        }
        // 생명 게이지 차감
        hp -= collision.gameObject.GetComponent<BulletCtrl>().damage;
        if(hp <= 0.0f)
        {
            GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
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
}
