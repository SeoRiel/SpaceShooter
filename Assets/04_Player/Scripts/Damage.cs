using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    private const string bulletTag = "BULLET";
    private const string enemyTag = "ENEMY";
    private float inializeHp = 100.0f;
    public float currentHp;

    // ��������Ʈ �� �̺�Ʈ ����
    public delegate void PlayerDieHander();
    public static event PlayerDieHander OnPlayerDie;

    // BloodScreen Texture�� �����ϱ� ���� ����
    public Image bloodScreen;

    // HP bar Image�� �����ϱ� ���� ����
    public Image hpBar;

    // HP bar�� ó�� ����
    private readonly Color initColor = new Vector4(0, 1.0f, 0.0f, 1.0f);
    private Color currentColor;

    // Start is called before the first frame update
    private void Start()
    {
        currentHp = inializeHp;

        // HP bar�� �ʱ� ���� ����
        hpBar.color = initColor;
        currentColor = initColor;
    }

    private void OnTriggerEnter(Collider collider)
    {
       // �浹�� Collider�� Tag�� IsTrigger �ɼ��� üũ���� �� �߻�
       if(collider.tag == bulletTag)
        {
            Destroy(collider.gameObject);

            // ���� ȿ���� ǥ���� �ڷ�ƾ �Լ� ȣ��
            StartCoroutine(ShowBloodScreen());

            currentHp -= 5.0f;
            // Debug.Log("Player HP : " + currentHp.ToString());

            // HP bar�� ���� �� ũ�� ���� �Լ� ȣ��
            DisplayHpBar();

            if(currentHp <= 0.0f)
            {
                PlayerDie();
            }
        }
    }

    private IEnumerator ShowBloodScreen()
    {
        // BloodScreen Texture�� Alpha ���� �ұ�Ģ�ϰ� ����
        bloodScreen.color = new Color(1, 0, 0, Random.Range(0.2f, 0.3f));
        yield return new WaitForSeconds(0.1f);

        // BloodScreen Texture�� ������ ��� 0���� ����
        bloodScreen.color = Color.clear;
    }

    // Player�� ��� ó�� ��ƾ
    private void PlayerDie()
    {
        OnPlayerDie();
        //  Debug.Log("Player Die!");
        // "ENEMY" Tag�� ������ ��� �� ĳ���͸� �����Ͽ� �迭�� ����
        // GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        // �迭�� ó������ ��ȸ�ϸ鼭 �� ĳ������ OnPlayDie Function ȣ��
        //for(int index = 0; index < enemies.Length; index++)
        //{
        //    enemies[index].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //}
    }

    private void DisplayHpBar()
    {
        // HP�� 50%�� �������� ������� ��������� ����
        if(currentHp / inializeHp > 0.5f)
        {
            currentColor.r = (1 - (currentHp / inializeHp)) * 2.0f;
        }
        // HP�� 0%�� �������� ��������� ���������� ����
        else
        {
            currentColor.g = (currentHp / inializeHp) * 2.0f;  
        }

        // HpBar�� ���� ����
        hpBar.color = currentColor;
        // HpBar�� ũ�� ����
        hpBar.fillAmount = (currentHp / inializeHp);
    }
}
