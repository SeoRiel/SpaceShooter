using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// �Ѿ� �߻�� ������ ����� Ŭ���� ������ ����ü
[System.Serializable]
public struct PlayerSFX
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}

public class FireCtrl : MonoBehaviour
{
    public enum WeaponType
    {
        RIFLE = 0,
        SHOUTGUN
    }

    public GameObject bullet;
    public Transform firePosition;
 // public ParticleSystem cartridge;                       // ź�� ���� ��ƼŬ
    public WeaponType currentWeapon = WeaponType.RIFLE;    // �÷��̾� ĳ���Ͱ� ���� ��� �ִ� ���⸦ ������ ����
    public PlayerSFX playerSFX;                            // ����� Ŭ���� ������ ����

    public Image magazineImage;                            // źâ �̹���
    public Text magazineText;                              // ���� �Ѿ� ����
    public Sprite[] weaponIcons;                           // ������ ���� �̹���
    public Image weaponImage;                              // ��ü�� ���� �̹��� UI

    public int maxBullet = 10;                             // �ִ� �Ѿ� ��
    public int remainingBullet = 10;                       // ���� �Ѿ� ��

    public float reloadTime = 2.0f;                        // ������ �ð�
    public bool isReloading = false;                       // ������ ���θ� �Ǵ��� ����

    private ParticleSystem muzzleFlash;                    // �ѱ� ȭ�� ��ƼŬ
    private AudioSource audio;                             // AudioSource Component�� ������ ����
    private ShakeCamera shake;                             // Shake Class�� ������ ����

    // Raycast
    public float fireRate = 0.1f;                           // �Ѿ��� �߻� ����

    private int enemyLayer;                                 // �� ĳ������ ���̾�� ������ ����
    private bool isFire = false;                            // �ڵ� �߻� ���θ� �Ǵ��� �Լ�
    private float nextFire;                                 // ���� �߻� �ð��� ������ ����
    private int obstacleLayer;                              // ��ֹ� ���̾�� ������ ����
    private int layerMask;                                  // ���̾� ����ũ�� ��Ʈ ������ ���� ����

    // Start is called before the first frame update
    private void Start()
    {
        // FirePosition ������ �ִ� Component ����
        muzzleFlash = firePosition.GetComponentInChildren<ParticleSystem>();

        // AudioSource Component ����
        audio = GetComponent<AudioSource>();

        // shake Scrpit ����
        shake = GameObject.Find("CameraRig").GetComponent<ShakeCamera>();

        // �� ĳ������ ���̾� ���� ����
        enemyLayer = LayerMask.NameToLayer("ENEMY");

        // ��ֹ��� ���̾� ���� ����
        obstacleLayer = LayerMask.NameToLayer("OBSTACLE");

        // ���̾� ����ũ�� ��Ʈ ���� (OR ����)
        layerMask = 1 << obstacleLayer | 1 << enemyLayer;
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.DrawRay(firePosition.position, firePosition.forward * 20.0f, Color.green);

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // ���� ĳ��Ʈ�� ����� ��ü�� ������ ������ ����
        RaycastHit hit;

        // ����ĳ��Ʈ�� ������ �� ĳ���͸� ����
        // Physics.Raycast(���� ��ǥ, �߻� ����, out ��ȯ ���� ����, ���� �Ÿ�, ������ ���̾�)
        if (Physics.Raycast(firePosition.position, firePosition.forward, out hit, 20.0f, 1 << enemyLayer))
        {
            isFire = (hit.collider.CompareTag("ENEMY"));
        }
        else
        {
            isFire = false;
        }

        if(!isReloading && isFire)
        {
            if(Time.time > nextFire)
            {
                // �Ѿ� �� �ϳ� ����
                --remainingBullet;
                Fire();

                // ���� �Ѿ��� ���� ��� ������ �ڷ�ƾ ȣ��
                if(remainingBullet == 0)
                {
                    StartCoroutine(Reloading());
                }
                // ���� �Ѿ� �߻� �ð��� ���
                nextFire = Time.time + fireRate;
            }
        }

        // ���콺 ���� ��ư�� Ŭ������ ��, Fire �Լ� ȣ��
        if (!isReloading && Input.GetMouseButtonDown(0))
        {
            // �Ѿ� ���� �ϳ� ����
            --remainingBullet;
            Fire();

            // ���� �Ѿ��� ���� ��� ������ �ڷ�ƾ ȣ��
            if (remainingBullet == 0)
            {
                StartCoroutine(Reloading());
            }
        }


    }

    private void Fire()
    {
        // Shake Effect ȣ��
        StartCoroutine(shake.CameraShake());

        // Bullet Prefab�� �������� ����
        // Instantiate(bullet, firePosition.position, firePosition.rotation);

        var _bullet = GameManager.instance.GetBullet();
        if (_bullet != null)
        {
            _bullet.transform.position = firePosition.position;
            _bullet.transform.rotation = firePosition.rotation;
            _bullet.SetActive(true);
        }

        // ��ƼŬ ����
        // cartridge.Play();

        // �ѱ� ȭ�� ��ƼŬ ����
        muzzleFlash.Play();

        // ������ �̹����� fillAmount �Ӽ��� ����
        magazineImage.fillAmount = (float)remainingBullet / (float)maxBullet;
        // ���� �Ѿ� �� ����
        UpdateBulletText();
    }

    private IEnumerator Reloading()
    {
        isReloading = true;
        audio.PlayOneShot(playerSFX.reload[(int)currentWeapon], 1.0f);

        // ������ ������� ���� + 0.3�� ���� ���
        yield return new WaitForSeconds(playerSFX.reload[(int)currentWeapon].length + 0.3f);

        // �� �� �������� �ʱ�ȭ
        isReloading = false;
        magazineImage.fillAmount = 1.0f;
        remainingBullet = maxBullet;
        // ���� �Ѿ� �� ����

        UpdateBulletText();
    }

    private void UpdateBulletText()
    {
        // ���� �Ѿ� �� / �ִ� �Ѿ� �� ǥ��
        magazineText.text = string.Format("<color=#ff0000>{0}</color>/{1}", remainingBullet, maxBullet);
    }

    private void FireSFX()
    {
        // ���� ��� �ִ� ������ ����� Ŭ�� ȣ��
        var sfx = playerSFX.fire[(int)currentWeapon];

        // ���� ���
        audio.PlayOneShot(sfx, 1.0f);
    }

    public void OnChangeWeapon()
    {
        currentWeapon = (WeaponType)((int)++currentWeapon % 2);
        weaponImage.sprite = weaponIcons[(int)currentWeapon];
    }
}
