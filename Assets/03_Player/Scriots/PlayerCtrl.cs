using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private float vertical = 0.0f;
    private float horizontal = 0.0f;
    private float rotate = 0.0f;

    private Transform playerTransform;                  // �����ؾ� �ϴ� ������Ʈ�� �ݵ�� ������ �Ҵ��� �� ���
    
    public float moveSpeed = 10.0f;                     // �̵� �ӵ� ����(public���� ����Ǿ� Inspector�� �����)
    public float rotateSpeed = 80.0f;                   // ȸ�� �ӵ� ����

    void Start()                                        // Start is called before the first frame update
    {
        playerTransform = GetComponent<Transform>();    // ��ũ��Ʈ�� ����� ��,
                                                        // ó�� ����Ǵ� Start �Լ����� Transform ������Ʈ �Ҵ�
    }

    void Update()                                       // Update is called once per frame
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        rotate = Input.GetAxis("Mouse X");

        Debug.Log("horizontal = " + horizontal.ToString());
        Debug.Log("Vertical = " + vertical.ToString());

        // �����¿� �̵� ���� ���� ���
        Vector3 moveDirection = (Vector3.forward * vertical) + (Vector3.right * horizontal);

        // Translate(�̵� ���� * �ӵ� * ������ * Time.deltaTime, ���� ��ǥ)
        playerTransform.Translate(Vector3.forward * moveSpeed * vertical * Time.deltaTime, Space.Self);

        // Vector3.up ���� �������� rotateSpeed��ŭ�� �ӵ��� ȸ��
        playerTransform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime * rotate);
    }
}
