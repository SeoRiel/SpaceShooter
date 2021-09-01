using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    public GameObject hpBarPrefab;
    public Vector3 hpBarOffset = new Vector3(0.0f, 0.0f, 0.0f);

    private const string bulletTag = "BULLET";  
    private float hp = 100.0f;                  // ü�� ������
    private float initHp = 100.0f;              // �ʱ� ü�� ������
    private GameObject bloodEffect;             // �ǰ� �� ����� ���� ȿ��

    private Canvas uiCanvas;                    // �θ� �� Canvas ��ü
    private Image hpbarImage;                   // ü�� �������� ���� fillAmount �Ӽ��� ������ Image

    // Start is called before the first frame update
    private void Start()
    {
        // EnemyAI ��ũ��Ʈ�� �����Ͽ� ������ ����
        // enemyAI = GetComponent<EnemyAI>();

        // ���� ȿ�� ������ �ε�
        bloodEffect = Resources.Load<GameObject>("BulletImpactFleshBigEffect");

        // ���� �������� ���� �� �ʱ�ȭ
        SetHpbar();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == bulletTag)
        {
            // ���� ȿ���� �����ϴ� �Լ� ȣ��
            ShowBloodEffect(collision);
            // �Ѿ� ����
            // Destroy(collision.gameObject);
            collision.gameObject.SetActive(false);

        }
        // ���� ������ ����
        hp -= collision.gameObject.GetComponent<BulletCtrl>().damage;
        // ���� �������� fillAmount �Ӽ��� ����
        hpbarImage.fillAmount = hp / initHp;
        
        if(hp <= 0.0f)
        {
            GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
            hpbarImage.GetComponentsInParent<Image>()[1].color = Color.clear;
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

    private void SetHpbar()
    {
        uiCanvas = GameObject.Find("UI Canvas").GetComponent<Canvas>();
        // UI Canvas ������ HP bar ����
        GameObject hpBar = Instantiate<GameObject>(hpBarPrefab, uiCanvas.transform);
        // fillAmount �Ӽ��� ������ Image�� ����
        hpbarImage = hpBar.GetComponentsInChildren<Image>()[1];

        // ���� �������� ���󰡾��� ���� ������ �� ����
        var chaseHpbar = hpBar.GetComponent<EnemyHpbar>();
        chaseHpbar.targetTransform = this.gameObject.transform;
        chaseHpbar.offset = hpBarOffset;
    }
}
