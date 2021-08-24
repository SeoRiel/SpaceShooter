using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private const string bulletTag = "BULLET";
    private const string enemyTag = "ENEMY";
    private float inializeHp = 100.0f;
    public float currentHp;

    // ��������Ʈ �� �̺�Ʈ ����
    public delegate void PlayerDieHander();
    public static event PlayerDieHander OnPlayerDie;

    // Start is called before the first frame update
    private void Start()
    {
        currentHp = inializeHp;
    }

    private void OnTriggerEnter(Collider collider)
    {
       // �浹�� Collider�� Tag�� IsTrigger �ɼ��� üũ���� �� �߻�
       if(collider.tag == bulletTag)
        {
            Destroy(collider.gameObject);

            currentHp -= 5.0f;
            Debug.Log("Player HP : " + currentHp.ToString());

            if(currentHp <= 0.0f)
            {
                PlayerDie();
            }
        }
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
}
