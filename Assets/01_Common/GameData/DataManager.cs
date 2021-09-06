using System.Collections;
using System.Collections.Generic;
// ���� ������� ���� namespace
using System.IO;
// ���̳ʸ� ���� ������ ���� namespace
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
// ������ ���� Ŭ������ �����ϱ� ���� namespace
using DataInfomation;

public class DataManager : MonoBehaviour
{
    public string dataPath;

    // ���� ��ο� ���ϸ� ����
    public void Initiailze()
    {
        dataPath = Application.persistentDataPath + "/gameData.dat";
    }

    // ������ ���� �� ������ �����ϴ� �Լ�
    public void Save(GameData gameData)
    {
        // ���̳ʸ� ���� ������ ���� BinaryFormatter ����
        BinaryFormatter binaryFomat = new BinaryFormatter();

        // ������ ������ ���� ���� ����
        FileStream file = File.Create(dataPath);

        // ���Ͽ� ������ Ŭ������ ������ �Ҵ�
        GameData data = new GameData();
        data.killCount = gameData.killCount;
        data.hp = gameData.hp;
        data.speed = gameData.speed;
        data.damage = gameData.damage;
        data.equipItem = gameData.equipItem;

        // Binary Formatter�� ����� ���Ͽ� ������ ���
        binaryFomat.Serialize(file, data);
        file.Close();
    }

    // ���Ͽ��� �����͸� �����ϴ� �Լ�
    public GameData Load()
    {
        if (File.Exists(dataPath))
        {
            // ������ �����ϴ� ��� ������ �ҷ�����
            BinaryFormatter binaryFormat = new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.Open);

            // GameData class�� ���Ϸκ��� ���� �����͸� ���
            GameData data = (GameData)binaryFormat.Deserialize(file);
            file.Close();

            return data;
        }
        else
        {
            // ������ ���� ��� �⺻������ ��ȯ
            GameData data = new GameData();

            return data;
        }
    }
}
