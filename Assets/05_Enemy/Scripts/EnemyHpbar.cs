using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpbar : MonoBehaviour
{
    private Camera uiCamera;           // Canvers�� �������ϴ� ī�޶�
    private Canvas canvas;             // UI�� �ֻ��� ĵ����
    private RectTransform rectParent;  // �θ� RectTransform Component
    private RectTransform rectHp;      // �ڽ��� RectTransform Component

    // HP bar �̹����� ��ġ�� ������ ������
    [HideInInspector] public Vector3 offset = Vector3.zero;

    // ������ ����� Transform Component
    [HideInInspector] public Transform targetTransform;

    private void Start()
    {
        // Component ���� �� �Ҵ�
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        // ���� ��ǥ�� ��ũ�� ��ǥ�� ��ȯ
        var screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position + offset);

        // ī�޶��� ���� ����(180�� ȸ��)�� �� ��ǥ�� ����
        if(screenPosition.z < 0.0f)
        {
            screenPosition *= -1.0f;
        }

        // RectTransform ��ǥ���� ���� ���� ����
        var localPosition = Vector2.zero;

        // ��ũ�� ��ǥ�� RectTransform ������ ��ǥ�� ��ȯ
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPosition, uiCamera, out localPosition);

        // ���� �������� ��ġ�� ����
        rectHp.localPosition = localPosition;
    }
}