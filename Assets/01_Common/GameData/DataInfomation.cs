using System.Collections.Generic;

namespace DataInfomation
{
    [System.Serializable]
    public class GameData
    {
        public int killCount = 0;                               // ����� �� ĳ������ ��
        public float hp = 120.0f;                               // ���ΰ��� �ʱ� ����
        public float damage = 25.0f;                            // �Ѿ��� ������
        public float speed = 6.0f;                              // �̵� �ӵ�
        public List<Item> equipItem = new List<Item>();         // ȹ���� ������
    }

    [System.Serializable]
    public class Item
    {
        public enum ItemType { HP, SPEED, GRENADE, DAMAGE };    // ������ ���� ����
        public enum ItemCalc { INC_VALUE, PERCENT };            // ��� ��� ����
        public ItemType itemType;                               // �������� ����
        public ItemCalc itemCalc;                               // ������ ���� �� ��� ���
        public string name;                                     // ������ �̸�
        public string desc;                                     // ������ �Ұ�
        public float value;                                     // ��� ��
    }
}
