using System.Collections;
using System.Collections.Generic;
// 파일 입출력을 위한 namespace
using System.IO;
// 바이너리 파일 포맷을 위한 namespace
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
// 데이터 저장 클래스에 접근하기 위한 namespace
using DataInfomation;

public class DataManager : MonoBehaviour
{
    public string dataPath;

    // 파일 경로와 파일명 지정
    public void Initiailze()
    {
        dataPath = Application.persistentDataPath + "/gameData.dat";
    }

    // 데이터 저장 및 파일을 생성하는 함수
    public void Save(GameData gameData)
    {
        // 바이너리 파일 포멧을 위한 BinaryFormatter 생성
        BinaryFormatter binaryFomat = new BinaryFormatter();

        // 데이터 저장을 위한 파일 생성
        FileStream file = File.Create(dataPath);

        // 파일에 저장할 클래스에 데이터 할당
        GameData data = new GameData();
        data.killCount = gameData.killCount;
        data.hp = gameData.hp;
        data.speed = gameData.speed;
        data.damage = gameData.damage;
        data.equipItem = gameData.equipItem;

        // Binary Formatter를 사용해 파일에 데이터 기록
        binaryFomat.Serialize(file, data);
        file.Close();
    }

    // 파일에서 데이터를 추출하는 함수
    public GameData Load()
    {
        if (File.Exists(dataPath))
        {
            // 파일이 존재하는 경우 데이터 불러오기
            BinaryFormatter binaryFormat = new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.Open);

            // GameData class에 파일로부터 읽은 데이터를 기록
            GameData data = (GameData)binaryFormat.Deserialize(file);
            file.Close();

            return data;
        }
        else
        {
            // 파일이 업는 경우 기본값으로 변환
            GameData data = new GameData();

            return data;
        }
    }
}
