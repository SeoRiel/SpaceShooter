using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    public bool isFire = false;                                         // �Ѿ� �߻� ���θ� �Ǵ��� ����
    public AudioClip fireSFX;                                           // ���ΰ��� ���� ȸ���� �ӵ� ���
    public AudioClip reloadSFX;                                         // ������ ���带 ������ ����
    public GameObject bulletObject;                                     // �� ĳ������ �Ѿ� ������
    public Transform firePosition;                                      // �Ѿ��� �߻� ��ġ ����
    public MeshRenderer muzzleFlash;                                    // MuzzleFlash�� MeshRenderer Component�� ������ ����

    private AudioSource audio;                                          // AudioSource Component�� ������ ����
    private Animator animator;                                          // Animator Component�� ������ ����
    private Transform playerTransform;                                  // �÷��̾� ĳ������ Transform Component�� ������ ����
    private Transform enemyTransform;                                   // �� ĳ������ Transform Component�� ������ ����

    // Animator Controller�� ������ �Ķ������ Hash Value�� �̸� ����
    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashReload = Animator.StringToHash("Reload");

    private float nextFire = 0.0f;                                      // ���� �߻��� �ð� ���� ����
    private readonly float fireRate = 0.1f;                             // �Ѿ� �߻� ����
    private readonly float damping = 10.0f;                             // ���ΰ��� ���� ȸ���� �ӵ� ���

    private readonly float reloadTime = 2.0f;                           // ������ �ð�
    private readonly int maxBullet = 10;                                // źâ�� �ִ� �Ѿ� ����
    private int currentBullet = 10;                                     // �ʱ� �Ѿ� ����
    private bool isReload = false;                                      // ������ ����
    private WaitForSeconds wsReload;                                    // ������ �ð� ���� ��ٸ� ���� ����

    private void Start()
    {
        // Component ���� �� ����
        playerTransform = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        enemyTransform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        wsReload = new WaitForSeconds(reloadTime);
    }

    private void Update()
    {
        if(!isReload && isFire)
        {
            // ���� �ð��� ���� �߻� �ð����� ū�� Ȯ��
            if(Time.time >= nextFire)
            {
                Fire();
                // ���� �߻� �ð� ���
                nextFire = Time.time + fireRate + Random.Range(0.0f, 0.3f);
            }

            // ���ΰ��� �ִ� ��ġ���� ȸ�� ���� ���
            Quaternion rotate = Quaternion.LookRotation(playerTransform.position - enemyTransform.position);
            // ���� �Լ��� �̿��� ������ ȸ��
            enemyTransform.rotation = Quaternion.Slerp(enemyTransform.rotation, rotate, Time.deltaTime * damping);
        }
    }

    private void Fire()
    {
        animator.SetTrigger(hashFire);
        audio.PlayOneShot(fireSFX, 1.0f);

        // �ѱ� ȭ�� ȿ�� �ڷ�ƾ ȣ��
        StartCoroutine(ShowMuzzleFlash());

        // �Ѿ� ����
        GameObject bullet = Instantiate(bulletObject, firePosition.position, firePosition.rotation);
        // ���� �ð��� ���� ��, ����
        Destroy(bullet, 3.0f);

        // ���� �Ѿ˷� ������ ���� ���
        isReload = (--currentBullet % maxBullet == 0);
        if(isReload)
        {
            StartCoroutine(Reloading());
        }
    }

    IEnumerator ShowMuzzleFlash()
    {
        // MuzzleFlash Ȱ��ȭ
        muzzleFlash.enabled = true;

        // �ұ�Ģ�� ȸ�� ���� ���
        Quaternion rotate = Quaternion.Euler(Vector3.forward * Random.Range(0, 360));
        // MuzzleFlash�� Z�� �������� ȸ��
        muzzleFlash.transform.localRotation = rotate;
        // MuzzleFlash�� �������� �ұ�Ģ�ϰ� ����
        muzzleFlash.transform.localScale = Vector3.one * Random.Range(1.0f, 2.0f);

        // �ؽ�ó�� Offset �Ӽ��� ������ �ұ�Ģ�� ���� ����
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;
        // MuzzleFlash�� Material�� Offset ���� ����
        muzzleFlash.material.SetTextureOffset("_MainTex", offset);                      // _MainTex == Diffuse, _BumpMap == Normal Map, _Cube == CubeMap

        // MuzzleFlash�� ���� ������ ��� ���
        yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        // MuzzleFlash�� �ٽ� ��Ȱ��ȭ
        muzzleFlash.enabled = false;
    }

    IEnumerator Reloading()
    {
        // MuzzleFlash�� ��Ȱ��ȭ
        muzzleFlash.enabled = false;

        // ������ �ִϸ��̼� ����
        animator.SetTrigger(hashReload);

        // ������ ���� ����
        audio.PlayOneShot(reloadSFX, 1.0f);

        // ������ �ð���ŭ ����ϴ� ���� ����� �絵
        yield return wsReload;

        // �Ѿ��� ���� �ʱ�ȭ
        currentBullet = maxBullet;
        isReload = false;
    }


}
