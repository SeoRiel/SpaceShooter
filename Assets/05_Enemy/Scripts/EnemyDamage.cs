using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private const string bulletTag = "BULLET";  
    private float hp = 100.0f;                  // ü�� ������
    private GameObject bloodEffect;             // �ǰ� �� ����� ���� ȿ��

    // Start is called before the first frame update
    private void Start()
    {
        // ���� ȿ�� ������ �ε�
        bloodEffect = Resources.Load<GameObject>("BulletImpactFleshBigEffect");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == bulletTag)
        {
            // ���� ȿ���� �����ϴ� �Լ� ȣ��
            ShowBloodEffect(collision);
            // �Ѿ� ����
            Destroy(collision.gameObject);
        }
        // ���� ������ ����
        hp -= collision.gameObject.GetComponent<BulletCtrl>().damage;
        if(hp <= 0.0f)
        {
            GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
        }
    }

    private void ShowBloodEffect(Collision collision)
    {
        // �Ѿ��� �浹�� ���� ����
        Vector3 hitPosition = collision.contacts[0].point;

        // �Ѿ��� �浹�Ͽ� �߻��� ���� ����
        Vector3 hitNormal = collision.contacts[0].normal;

        // �Ѿ� �浹 �� ���� ������ ȸ���� ���
        Quaternion rotate = Quaternion.FromToRotation(-Vector3.forward, hitNormal);

        // ���� ȿ�� ����
        GameObject blood = Instantiate<GameObject>(bloodEffect, hitPosition, rotate);
        Destroy(blood, 1.0f);
    }
}
