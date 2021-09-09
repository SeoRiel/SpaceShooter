using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;                // 추적 대상
    public float moveDamping = 15.0f;       // 이동 속도 계수
    public float rotateDamping = 10.0f;     // 회전 속도 계수
    public float distance = 5.0f;           // 추적 대상과의 거리
    public float height = 4.0f;             // 추적 대상과의 높이
    public float targetOffset = 2.0f;       // 추적 좌표의 오프셋

    private Transform camTransform;         // CameraRig의 Transform Component

    [Header("Wall Obstacle Setting")]       
    public float heightAboveWall = 7.0f;    // 카메라가 올라갈 높이
    public float colliderRadius = 1.8f;     // 충돌체의 반지름
    public float overDamping = 5.0f;        // 이동 속도 계수
    public float originHeight;              // 최초 높이를 보관할 변수

    [Header("Etc Obstacle Setting")]
    public float heightAboveObstacle = 12.0f;   // 카메라가 올라갈 높이
    public float castOffset = 1.0f;             // 주인공에 투사할 레이 캐스트의 높이 오프셋

    // Start is called before the first frame update
    private void Start()
    {
        // CameraRig의 transform Component 추출
        camTransform = GetComponent<Transform>();

        // 최초의 카메라 높이 지정
        originHeight = height;
    }

    private void Update()
    {
        // 구체 형태의 충돌체로 충돌 여부 검사
        if(Physics.CheckSphere(transform.position, colliderRadius))
        {
            // 보간 함수를 이용하여 카메라의 높이를 부드럽게 증가
            height = Mathf.Lerp(height, heightAboveWall, Time.deltaTime * overDamping);
        }
        else
        {
            // 보간 함수를 이용하여 카메라의 높이를 부드럽게 감소
            height = Mathf.Lerp(height, originHeight, Time.deltaTime * overDamping);
        }

        // 주인공이 장애물에 가려졌는지 판단할 RayCast의 높낮이 설정
        Vector3 castTarget = target.position + (target.up * castOffset);

        // castTarget 좌표의 방향 벡터 계산
        Vector3 castDirection = (castTarget - transform.position).normalized;

        // 충돌 정보를 반환 받을 변수
        RaycastHit rayHit;

        if(Physics.Raycast(camTransform.position, castDirection, out rayHit, Mathf.Infinity))
        {
            if(!rayHit.collider.CompareTag("PLAYER"))
            {
                height = Mathf.Lerp(height, heightAboveObstacle, Time.deltaTime * overDamping);
            }
            else
            {
                height = Mathf.Lerp(height, originHeight, Time.deltaTime * overDamping);
            }
        }

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
        // Gizmo의 색상 지정
        Gizmos.color = Color.green;

        // 추적 및 시야를 맞출 위치를 표시
        Gizmos.DrawWireSphere(target.position + (target.up * targetOffset), 0.1f);

        // 메인 카메라와 추적 지점 간의 선을 표시
        Gizmos.DrawLine(target.position + (target.up * targetOffset), transform.position);

        // 카메라의 충돌체를 표현하기 위한 구체 표시
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, colliderRadius);

        // Player Character가 장애물에 가려졌는지 판단한 레이를 표시
        Gizmos.color = Color.red;
        Gizmos.DrawLine(target.position + (target.up * castOffset), transform.position);
    }
}