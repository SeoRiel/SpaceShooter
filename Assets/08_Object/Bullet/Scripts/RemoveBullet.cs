using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject sparkEffect;                  // Spark Prefab�� ������ ����

    // �浹�� ������ �� �߻��ϴ� �̺�Ʈ
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "BULLET")      // �浹�� ���� ������Ʈ�� �±װ� ��
        {
            ShowEffect(collision);                  // Call is spark effect function
            // Destroy(collision.gameObject);          // �浹�� ���� ������Ʈ ����
            collision.gameObject.SetActive(false);
        }
    }

    private void ShowEffect(Collision collision)
    {
        // �浹 ������ ���� ����
        ContactPoint contact = collision.contacts[0];

        // ���� ���Ͱ� �̷�� ȸ�� ���� ����
        Quaternion rotation = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

        // ����ũ ȿ�� ����
        GameObject spark = Instantiate(sparkEffect, contact.point + (-contact.normal * 0.05f), rotation);

        // ����ũ ȿ���� �θ� �巳�� �Ǵ� ������ ����
        spark.transform.SetParent(this.transform);
    }
}

