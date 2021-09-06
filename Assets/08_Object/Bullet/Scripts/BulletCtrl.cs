using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float damage = 20.0f;                           // �Ѿ��� ������
    public float speed = 1000.0f;                          // �Ѿ��� �߻� �ӵ�

    private Transform tr;
    private Rigidbody rb;
    private TrailRenderer trail;

    private void Awake()
    {
        // ������Ʈ �Ҵ�
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();

        // �ҷ��� ������ ���� damage�� ����
        damage = GameManager.instance.gameData.damage;
    }

    // Start is called before the first frame update
    //private void Start()
    //{
    //    // FirePosition ������ �ִ� Component ����
    //    GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    //}

    private void OnEnable()
    {
        rb.AddForce(transform.forward * speed);
    }

    private void OnDisable()
    {
        // ��Ȱ��� �Ѿ��� ���� ȿ���� �ʱ�ȭ
        trail.Clear();
        tr.position = Vector3.zero;
        tr.rotation = Quaternion.identity;
        rb.Sleep();
    }
}
