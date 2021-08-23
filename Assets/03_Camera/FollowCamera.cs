using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;            // 추적 대상
    public float moveDamping = 15.0f;   // 이동 속도 계수
    public float rotateDamping = 10.0f; // 회전 속도 계수
    public float distance = 5.0f;       // 추적 대상과의 거리
    public float height = 4.0f;         // 추적 대상과의 높이
    public float targetOffset = 2.0f;   // 추적 좌표의 오프셋

    private Transform camTransform;        // CameraRig의 Transform Component

    // Start is called before the first frame update
    private void Start()
    {
        // CameraRig의 transform Component 추출
        camTransform = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        // 카메라의 높이와 거리를 계산
        var cameraPosition = target.position - (target.forward * distance) + (target.up * height);

        // 이동할 때의 속도 계수를 적용
        camTransform.position = Vector3.Slerp(transform.position, cameraPosition, Time.deltaTime * moveDamping);

        // 회전할 때의 속도 계수를 적용
        camTransform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, Time.deltaTime * rotateDamping);

        // 카메라를 추적 대상으로 Z축을 회전시킴
        camTransform.LookAt(target.position + (target.up * targetOffset));
    }

    // 추적할 좌표를 시각적으로 표현
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        // 추적 및 시야를 맞출 위치를 표시
        Gizmos.DrawWireSphere(target.position + (target.up * targetOffset), 0.1f);

        // 메인 카메라와 추적 지점 간의 선을 표시
        Gizmos.DrawLine(target.position + (target.up * targetOffset), transform.position);
    }
}