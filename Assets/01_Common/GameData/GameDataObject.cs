using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Item Class�� �����ϱ� ���� ����� namespace
using DataInfomation;

// �ּ� �޴��� ����ϱ� ���� ��Ʈ����Ʈ
[CreateAssetMenu(fileName = "GameDataSO", menuName = "Create GameData", order = 1)]

public class GameDataObject : ScriptableObject
{
    public int killCount = 0;                               // ����� �� ĳ������ ��
    public float hp = 120.0f;                               // ���ΰ��� �ʱ� ����
    public float damage = 25.0f;                            // �Ѿ��� ������
    public float speed = 6.0f;                              // �̵� �ӵ�
    public List<Item> equipItem = new List<Item>();         // ȹ���� ������
}
