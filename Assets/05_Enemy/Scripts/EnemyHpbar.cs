using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpbar : MonoBehaviour
{
    private Camera uiCamera;           // Canvers를 렌더링하는 카메라
    private Canvas canvas;             // UI용 최상위 캔버스
    private RectTransform rectParent;  // 부모 RectTransform Component
    private RectTransform rectHp;      // 자신의 RectTransform Component

    // HP bar 이미지의 위치를 조절할 프리셋
    [HideInInspector] public Vector3 offset = Vector3.zero;

    // 추적할 대상의 Transform Component
    [HideInInspector] public Transform targetTransform;

    private void Start()
    {
        // Component 추출 및 할당
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        // 월드 좌표를 스크린 좌표로 변환
        var screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position + offset);

        // 카메라의 뒤쪽 영역(180도 회전)일 때 좌표값 변경
        if(screenPosition.z < 0.0f)
        {
            screenPosition *= -1.0f;
        }

        // RectTransform 좌표값을 전달 받을 변수
        var localPosition = Vector2.zero;

        // 스크린 좌표를 RectTransform 기준의 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPosition, uiCamera, out localPosition);

        // 생명 게이지의 위치를 변경
        rectHp.localPosition = localPosition;
    }
}