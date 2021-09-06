using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float damage = 20.0f;                           // 총알의 데미지
    public float speed = 1000.0f;                          // 총알의 발사 속도

    private Transform tr;
    private Rigidbody rb;
    private TrailRenderer trail;

    private void Awake()
    {
        // 컴포넌트 할당
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();

        // 불러온 데이터 값을 damage에 적용
        damage = GameManager.instance.gameData.damage;
    }

    // Start is called before the first frame update
    //private void Start()
    //{
    //    // FirePosition 하위에 있는 Component 추출
    //    GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    //}

    private void OnEnable()
    {
        rb.AddForce(transform.forward * speed);
    }

    private void OnDisable()
    {
        // 재활용된 총알의 여러 효과값 초기화
        trail.Clear();
        tr.position = Vector3.zero;
        tr.rotation = Quaternion.identity;
        rb.Sleep();
    }
}
