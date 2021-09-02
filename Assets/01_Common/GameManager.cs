using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Enemy Create Info")]
    public Transform[] points;          // �� ĳ���Ͱ� ������ ��ġ�� ���� �迭
    public GameObject enemy;            // �� ĳ���� �������� ������ ����
    public float createTime = 2.0f;     // �� ĳ���͸� ������ �ֱ�
    public int maxEnemy = 10;           // �� ĳ������ �ִ� ���� ����
    public bool isGameOver = false;     // ���� ���� ���θ� �Ǵ��� ����

    // �̱��Ͽ� �����ϱ� ���� instance ���� ����
    public static GameManager instance = null;

    [Header("Obejct Pool")]
    // ������ �Ѿ� ������
    public GameObject bulletPrefabs;
    // ������Ʈ Ǯ�� ������ ����
    public int maxPool = 10;
    public List<GameObject> bulletPool = new List<GameObject>();

    private bool isPaused;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        // instance�� �Ҵ�� Ŭ������ �ν��Ͻ��� �ٸ� ��� ���� ������ Ŭ������ �ǹ���
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }

        // �ٸ� ������ �Ѿ���� �������� �ʰ� ����
        DontDestroyOnLoad(this.gameObject);

        // ������Ʈ Ǯ�� ���� �Լ� ȣ��
        CreatePooling();
    }

    // Start is called before the first frame update
    private void Start()
    {
        // ���̷�Ű ���� SpawnPointGroup�� ã�� ������ �ִ� ��� Transform Component�� ������ ��
        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();

        if(points.Length > 0)
        {
            StartCoroutine(this.CreateEnemy());
        }
    }

    // �� ĳ���͸� �����ϴ� �ڷ�ƾ �Լ�
    private IEnumerator CreateEnemy()
    {
        // ���� ���� �ñ��� ���� ����
        while(!isGameOver)
        {
            // ���� ������ �� ĳ������ ���� ����
            int enemyCount = (int)GameObject.FindGameObjectsWithTag("ENEMY").Length;

            // �� ĳ������ �ִ� ���� �������� ���� ���� �� ĳ���͸� ����
            if(enemyCount < maxEnemy)
            {
                // �� ĳ������ ���� �ֱ� �ð���ŭ ���
                yield return new WaitForSeconds(createTime);

                // �ұ�Ģ���� ��ġ ����
                int index = Random.Range(1, points.Length);

                // �� ĳ������ ���� ����
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
            // ��Ȱ��ȭ ���η� ��� ������ �Ѿ����� �Ǵ�
            if (bulletPool[i].activeSelf == false)
            {
                return bulletPool[i];
            }
        }
        return null;
    }

    // ������Ʈ Ǯ�� �Ѿ��� �����ϴ� �Լ�
    public void CreatePooling()
    {
        // �Ѿ��� ������ ���ϵ�ȭ�� ��Ʈ ���� ������Ʈ�� ����
        GameObject objectPools = new GameObject("ObjectPools");
        // Ǯ�� ������ŭ �Ѿ� ����
        for(int i = 0; i < maxPool; i++)
        {
            var bulletObject = Instantiate<GameObject>(bulletPrefabs, objectPools.transform);
            bulletObject.name = "Bullet_" + i.ToString("00");
            // ��Ȱ��ȭ
            bulletObject.SetActive(false);
            // ����Ʈ�� ������ �Ѿ� �߰�
            bulletPool.Add(bulletObject);
        }
    }

    public void OnPauseClick()
    {
        // �Ͻ� ���� ���� ��� ��Ŵ
        isPaused = !isPaused;

        // Time Scale�� 0�̸� ����, 1�̸� ���� �ӵ�
        Time.timeScale = (isPaused) ? 0.0f : 1.0f;

        // ���ΰ� ��ü ����
        var playerObject = GameObject.FindGameObjectWithTag("PLAYER");

        // ���ΰ� ĳ���Ϳ� �߰��� ��� ��ũ��Ʈ ����
        var scripts = playerObject.GetComponents<MonoBehaviour>();

        // ���ΰ� ĳ������ ��� ��ũ��Ʈ�� Ȱ��ȭ/��Ȱ��ȭ
        foreach(var script in scripts)
        {
            script.enabled = !isPaused;
        }

        var canvasGroup = GameObject.Find("Panel - Weapon").GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = !isPaused;
    }
}
