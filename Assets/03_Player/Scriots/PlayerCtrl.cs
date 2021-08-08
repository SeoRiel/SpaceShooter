using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private float vertical = 0.0f;
    private float horizontal = 0.0f;
    private float rotate = 0.0f;

    private Transform playerTransform;                  // 접근해야 하는 컴포넌트는 반드시 변수에 할당한 후 사용
    
    public float moveSpeed = 10.0f;                     // 이동 속도 변수(public으로 선언되어 Inspector에 노출됨)
    public float rotateSpeed = 80.0f;                   // 회전 속도 변수

    void Start()                                        // Start is called before the first frame update
    {
        playerTransform = GetComponent<Transform>();    // 스크립트가 실행된 후,
                                                        // 처음 수행되는 Start 함수에서 Transform 컴포넌트 할당
    }

    void Update()                                       // Update is called once per frame
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        rotate = Input.GetAxis("Mouse X");

        Debug.Log("horizontal = " + horizontal.ToString());
        Debug.Log("Vertical = " + vertical.ToString());

        // 전후좌우 이동 방향 벡터 계산
        Vector3 moveDirection = (Vector3.forward * vertical) + (Vector3.right * horizontal);

        // Translate(이동 방향 * 속도 * 변위값 * Time.deltaTime, 기준 좌표)
        playerTransform.Translate(Vector3.forward * moveSpeed * vertical * Time.deltaTime, Space.Self);

        // Vector3.up 축을 기준으로 rotateSpeed만큼의 속도로 회전
        playerTransform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime * rotate);
    }
}
