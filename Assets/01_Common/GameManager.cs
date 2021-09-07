using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// GameData, Item class가 들어간 namespace 명시
using DataInfomation;

public class GameManager : MonoBehaviour
{
    [Header("Enemy Create Info")]
    public Transform[] points;          // 적 캐릭터가 출현할 위치를 담을 배열
    public GameObject enemy;            // 적 캐릭터 프리팹을 저장할 변수
    public float createTime = 2.0f;     // 적 캐릭터를 생성할 주기
    public int maxEnemy = 10;           // 적 캐릭터의 최대 생성 개수
    public bool isGameOver = false;     // 게임 종료 여부를 판단할 변수
    public CanvasGroup inventoryCG;     // Inventory의 CanvasGroup Component를 저장할 변수

    // 싱글턴에 접근하기 위한 instance 변수 선언
    public static GameManager instance = null;

    // 인벤토리의 아이템이 변경되었을 때 발생시킬 이벤트의 정의
    public delegate void ItemChangeDelegate();
    public static event ItemChangeDelegate OnItemChange;

    // 주인공이 죽인 적 캐릭터의 수
    // [HideInInspector] public int killCount;
    [Header("GameData")]
    public Text killCountText;          // 적 캐릭터를 죽인 횟수를 표시할 텍스트 UI
    public GameData gameData;

    [Header("Obejct Pool")]
    public GameObject bulletPrefabs;    // 생성할 총알 프리팹
    public int maxPool = 10;            // 오브젝트 풀에 생성할 개수
    public List<GameObject> bulletPool = new List<GameObject>();

    private bool isPaused;
    private DataManager dataManager;    // DataManager를 저장할 변수

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        // instance에 할당된 클래스의 인스턴스가 다를 경우 새로 생성된 클래스를 의미함
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }

        // 다른 씬으로 넘어가더라도 삭제하지 않고 유지
        DontDestroyOnLoad(this.gameObject);

        // DataManager를 추출하여 저장
        dataManager = GetComponent<DataManager>();

        // dataManager 초기화
        dataManager.Initialize();

        // 게임의 초기 데이터 로드
        LoadGameData();

        // 오브젝트 풀링 생성 함수 호출
        CreatePooling();
    }

    // 게임의 초기 데이터 로드
    private void LoadGameData()
    {
        // GameData를 통해 파일에 저장된 데이터 불러오기
        GameData data = dataManager.Load();

        gameData.hp = data.hp;
        gameData.damage = data.damage;
        gameData.speed = data.speed;
        gameData.killCount = data.killCount;
        gameData.equipItem = data.equipItem;

        // KILL_COUNT 키로 저장된 값을 로드
        // killCount = PlayerPrefs.GetInt("KILL_COUNT", 0);
        // killCountText.text = "KILL " + killCount.ToString("0000");
        killCountText.text = "KILL " + gameData.killCount.ToString("0000");
    }

    // 게임 데이터를 저장
    private void SaveGameData()
    {
        dataManager.Save(gameData);
    }

    // 인벤토리 아이템을 추가했을 때, 데이터의 정보를 갱신하는 함수
    public void AddItem(Item item)
    {
        // 보유 아이템에 같은 아이템이 있으면 추가하지 않고 빠져나감
        if(gameData.equipItem.Contains(item))
        {
            return;
        }

        // 아이템을 GameData.equipItem 배열에 추가
        gameData.equipItem.Add(item);

        // 아이템의 종류에 따라 분기 처리
        switch(item.itemType)
        {
            case Item.ItemType.HP:
            {
                if(item.itemCalc == Item.ItemCalc.INC_VALUE)
                {
                    gameData.hp += item.value;
                }
                else
                {
                    gameData.hp += gameData.hp * item.value;
                }
                break;
            }

            case Item.ItemType.DAMAGE:
            {
                if(item.itemCalc == Item.ItemCalc.INC_VALUE)
                {
                    gameData.damage += item.value;
                }
                else
                {
                    gameData.damage += gameData.damage * item.value;
                }
                break;
            }

            case Item.ItemType.SPEED:
            {
                if(item.itemCalc == Item.ItemCalc.INC_VALUE)
                {
                    gameData.speed += item.value;
                }
                else
                {
                    gameData.speed += gameData.speed * item.value;
                }
                break;
            }

            case Item.ItemType.GRENADE:
            {
                break;
            }
        }
        // 아이템이 변경된 것을 실시간으로 반영하기 위해 이벤트를 발생시킴
        OnItemChange();
    }

    // 인벤토리에서 아이템을 제거했을 때 데이터를 갱신하는 함수
    public void RemoveItem(Item item)
    {
        // 아이템을 GameData.equipItem 배열에서 삭제
        gameData.equipItem.Remove(item);

        // 아이템 종류에 따른 분기 처리
        switch(item.itemType)
        {
            case Item.ItemType.HP:
            {
                // 아이템의 계산 방식에 따라 연산 처리
                if(item.itemCalc == Item.ItemCalc.INC_VALUE)
                {
                    gameData.hp -= item.value;
                }
                else
                {
                    gameData.hp = gameData.hp / (1.0f + item.value);
                }
                break;
            }

            case Item.ItemType.DAMAGE:
            {
                if(item.itemCalc == Item.ItemCalc.INC_VALUE)
                {
                    gameData.damage -= item.value;
                }
                else
                {
                    gameData.damage = gameData.damage / (1.0f + item.value);
                }
                break;
            }

            case Item.ItemType.SPEED:
            {
                if(item.itemCalc == Item.ItemCalc.INC_VALUE)
                {
                    gameData.speed -= item.value;
                }
                else
                {
                    gameData.speed = gameData.speed / (1.0f + item.value);
                }
                break;
            }

            case Item.ItemType.GRENADE:
            {
                break;
            }
        }
        // 아이템이 변경된 것을 실시간으로 반영하기 위해 이벤트 발생
        OnItemChange();
    }

    // Start is called before the first frame update
    private void Start()
    {
        // 처음 인벤토리를 비활성화
        OnInventoryOpen(false);

        // 하이러키 뷰의 SpawnPointGroup을 찾아 하위에 있는 모든 Transform Component를 가지고 옴
        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();

        if(points.Length > 0)
        {
            StartCoroutine(this.CreateEnemy());
        }
    }

    // 적 캐릭터를 생성하는 코루틴 함수
    private IEnumerator CreateEnemy()
    {
        // 게임 종료 시까지 무한 루프
        while(!isGameOver)
        {
            // 현재 생성된 적 캐릭터의 개수 산출
            int enemyCount = (int)GameObject.FindGameObjectsWithTag("ENEMY").Length;

            // 적 캐릭터의 최대 생성 개수보다 적을 때만 적 캐릭터를 생성
            if(enemyCount < maxEnemy)
            {
                // 적 캐릭터의 생성 주기 시간만큼 대기
                yield return new WaitForSeconds(createTime);

                // 불규칙적인 위치 산출
                int index = Random.Range(1, points.Length);

                // 적 캐릭터의 동적 생성
                Instantiate(enemy, points[index].position, points[index].rotation);
            }
            else
            {
                yield return null;
            }
        }
    }

    public GameObject GetBullet()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            // 비활성화 여부로 사용 가능한 총알인지 판단
            if (bulletPool[i].activeSelf == false)
            {
                return bulletPool[i];
            }
        }
        return null;
    }

    // 오브젝트 풀에 총알을 생성하는 함수
    public void CreatePooling()
    {
        // 총알을 생성해 차일드화할 페어런트 게임 오브젝트를 생성
        GameObject objectPools = new GameObject("ObjectPools");

        // 풀링 개수만큼 총알 생성
        for(int i = 0; i < maxPool; i++)
        {
            var bulletObject = Instantiate<GameObject>(bulletPrefabs, objectPools.transform);
            bulletObject.name = "Bullet_" + i.ToString("00");
            // 비활성화
            bulletObject.SetActive(false);
            // 리스트에 생성한 총알 추가
            bulletPool.Add(bulletObject);
        }
    }

    public void OnPauseClick()
    {
        // 일시 정지 값을 토글 시킴
        isPaused = !isPaused;

        // Time Scale이 0이면 정지, 1이면 정상 속도
        Time.timeScale = (isPaused) ? 0.0f : 1.0f;

        // 주인공 객체 추출
        var playerObject = GameObject.FindGameObjectWithTag("PLAYER");

        // 주인공 캐릭터에 추가된 모든 스크립트 추출
        var scripts = playerObject.GetComponents<MonoBehaviour>();

        // 주인공 캐릭터의 모든 스크립트를 활성화/비활성화
        foreach(var script in scripts)
        {
            script.enabled = !isPaused;
        }

        var canvasGroup = GameObject.Find("Panel - Weapon").GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = !isPaused;
    }

    // 인벤토리를 활성화/비활성화 하는 함수
    public void OnInventoryOpen(bool isOpened)
    {
        inventoryCG.alpha = (isOpened) ? 1.0f : 0.0f;
        inventoryCG.interactable = isOpened;
        inventoryCG.blocksRaycasts = isOpened;
    }

    // 적 캐릭터가 죽을 때마다 호출될 함수
    public void IncKillCount()
    {
        // ++killCount;
        // killCountText.text = "KILL " + killCount.ToString("0000");

        ++gameData.killCount;
        killCountText.text = "KILL " + gameData.killCount.ToString("0000");

        // 죽인 횟수 저장
        // PlayerPrefs.SetInt("KILL_COUNT", killCount);
    }

    private void OnApplicationQuit()
    {
        // 게임 종료 전, 게임 데이터를 저장
        SaveGameData();
    }
}
