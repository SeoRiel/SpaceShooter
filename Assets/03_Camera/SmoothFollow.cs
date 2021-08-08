using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;            // ������ Ÿ�� ������Ʈ�� Transform
    public float distance = 10.0f;      // ī�޶���� �Ÿ�
    public float height = 5.0f;         // ī�޶� ����
    public float smoothRotate = 5.0f;   // �ε巯�� ȸ���� ���� ����

    private Transform camTransform;     // ī�޶��� Transform ����

    // Start is called before the first frame update
    void Start()
    {
        camTransform = GetComponent<Transform>();        
    }

    // Update is called once per frame
    void Update()
    {
        // ī�޶��� �ε巯�� ȸ���� ���� Mathf.LerpAngle
        float currentAngle = 
            Mathf.LerpAngle(camTransform.eulerAngles.y, target.eulerAngles.y, smoothRotate * Time.deltaTime);


        // ���Ϸ� Ÿ���� ���ʹϾ����� ����
        Quaternion rotate = Quaternion.Euler(0, currentAngle, 0);

        // ī�޶� ��ġ�� Ÿ�� ȸ�� ������ŭ ȸ�� ��, distance��ŭ ����, hegiht��ŭ �ø���
        camTransform.position 
            = target.position - (rotate * Vector3.forward * distance) + (Vector3.up * height);

        // target�� �ٶ󺸵��� ����
        camTransform.LookAt(target);
    }
}
