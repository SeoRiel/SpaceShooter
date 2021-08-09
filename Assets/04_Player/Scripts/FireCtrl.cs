using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // public ParticleSystem cartridge;                    // ź�� ���� ��ƼŬ
    public WeaponType currentWeapon = WeaponType.RIFLE;    // �÷��̾� ĳ���Ͱ� ���� ��� �ִ� ���⸦ ������ ����
    public PlayerSFX playerSFX;                            // ����� Ŭ���� ������ ����

    private ParticleSystem muzzleFlash;                    // �ѱ� ȭ�� ��ƼŬ
    private AudioSource audio;                             // AudioSource Component�� ������ ����

    // Start is called before the first frame update
    private void Start()
    {
        // FirePosition ������ �ִ� Component ����
        muzzleFlash = firePosition.GetComponentInChildren<ParticleSystem>();

        // AudioSource Component ����
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        // ���콺 ���� ��ư�� Ŭ������ ��, Fire �Լ� ȣ��
        if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    private void Fire()
    {
        // Bullet Prefab�� �������� ����
        Instantiate(bullet, firePosition.position, firePosition.rotation);

        // ��ƼŬ ����
        // cartridge.Play();

        // �ѱ� ȭ�� ��ƼŬ ����
        muzzleFlash.Play();
    }

    private void FireSFX()
    {
        // ���� ��� �ִ� ������ ����� Ŭ�� ȣ��
        var sfx = playerSFX.fire[(int)currentWeapon];

        // ���� ���
        audio.PlayOneShot(sfx, 1.0f);
    }
}
