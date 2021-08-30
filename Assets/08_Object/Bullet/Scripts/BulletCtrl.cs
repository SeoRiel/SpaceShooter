using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float damage = 20.0f;                           // �Ѿ��� ������
    public float speed = 1000.0f;                          // �Ѿ��� �߻� �ӵ�

    // Start is called before the first frame update
    private void Start()
    {
        // FirePosition ������ �ִ� Component ����
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }
}
