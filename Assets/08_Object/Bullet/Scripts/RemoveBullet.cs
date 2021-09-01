using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject sparkEffect;                  // Spark Prefab을 저장할 변수

    // 충돌이 시작할 때 발생하는 이벤트
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "BULLET")      // 충돌한 게임 오브젝트의 태그값 비교
        {
            ShowEffect(collision);                  // Call is spark effect function
            // Destroy(collision.gameObject);          // 충돌한 게임 오브젝트 삭제
            collision.gameObject.SetActive(false);
        }
    }

    private void ShowEffect(Collision collision)
    {
        // 충돌 지점의 정보 추출
        ContactPoint contact = collision.contacts[0];

        // 법선 벡터가 이루는 회전 각도 추출
        Quaternion rotation = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

        // 스파크 효과 생성
        GameObject spark = Instantiate(sparkEffect, contact.point + (-contact.normal * 0.05f), rotation);

        // 스파크 효과의 부모를 드럼통 또는 벽으로 설정
        spark.transform.SetParent(this.transform);
    }
}

