using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float damage = 20.0f;                           // 총알의 데미지
    public float speed = 1000.0f;                          // 총알의 발사 속도

    // Start is called before the first frame update
    private void Start()
    {
        // FirePosition 하위에 있는 Component 추출
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }
}
