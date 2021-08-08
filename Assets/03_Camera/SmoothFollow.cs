using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;            // 추적할 타겟 오브젝트의 Transform
    public float distance = 10.0f;      // 카메라와의 거리
    public float height = 5.0f;         // 카메라 높이
    public float smoothRotate = 5.0f;   // 부드러운 회전을 위한 변수

    private Transform camTransform;     // 카메라의 Transform 변수

    // Start is called before the first frame update
    void Start()
    {
        camTransform = GetComponent<Transform>();        
    }

    // Update is called once per frame
    void Update()
    {
        // 카메라의 부드러운 회전을 위한 Mathf.LerpAngle
        float currentAngle = 
            Mathf.LerpAngle(camTransform.eulerAngles.y, target.eulerAngles.y, smoothRotate * Time.deltaTime);


        // 오일러 타입을 쿼터니언으로 변경
        Quaternion rotate = Quaternion.Euler(0, currentAngle, 0);

        // 카메라 위치를 타겟 회전 각도만큼 회전 후, distance만큼 띄우고, hegiht만큼 올리기
        camTransform.position 
            = target.position - (rotate * Vector3.forward * distance) + (Vector3.up * height);

        // target을 바라보도록 설정
        camTransform.LookAt(target);
    }
}
