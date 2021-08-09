using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 총알 발사와 재장전 오디오 클립을 저장할 구조체
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
    // public ParticleSystem cartridge;                    // 탄피 추출 파티클
    public WeaponType currentWeapon = WeaponType.RIFLE;    // 플레이어 캐릭터가 현재 들고 있는 무기를 저장할 변수
    public PlayerSFX playerSFX;                            // 오디오 클립을 저장할 변수

    private ParticleSystem muzzleFlash;                    // 총구 화염 파티클
    private AudioSource audio;                             // AudioSource Component를 저장할 변수

    // Start is called before the first frame update
    private void Start()
    {
        // FirePosition 하위에 있는 Component 추출
        muzzleFlash = firePosition.GetComponentInChildren<ParticleSystem>();

        // AudioSource Component 추출
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        // 마우스 왼쪽 버튼을 클릭했을 때, Fire 함수 호출
        if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    private void Fire()
    {
        // Bullet Prefab을 동적으로 생성
        Instantiate(bullet, firePosition.position, firePosition.rotation);

        // 파티클 실행
        // cartridge.Play();

        // 총구 화염 파티클 실행
        muzzleFlash.Play();
    }

    private void FireSFX()
    {
        // 현재 들고 있는 무기의 오디오 클립 호출
        var sfx = playerSFX.fire[(int)currentWeapon];

        // 사운드 출력
        audio.PlayOneShot(sfx, 1.0f);
    }
}
