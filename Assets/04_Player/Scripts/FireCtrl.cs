using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
 // public ParticleSystem cartridge;                       // 탄피 추출 파티클
    public WeaponType currentWeapon = WeaponType.RIFLE;    // 플레이어 캐릭터가 현재 들고 있는 무기를 저장할 변수
    public PlayerSFX playerSFX;                            // 오디오 클립을 저장할 변수

    public Image magazineImage;                            // 탄창 이미지
    public Text magazineText;                              // 남은 총알 개수

    public int maxBullet = 10;                             // 최대 총알 수
    public int remainingBullet = 10;                       // 남은 총알 수

    public float reloadTime = 2.0f;                        // 재장전 시간
    public bool isReloading = false;                       // 재장전 여부를 판단할 변수

    private ParticleSystem muzzleFlash;                    // 총구 화염 파티클
    private AudioSource audio;                             // AudioSource Component를 저장할 변수
    private ShakeCamera shake;                             // Shake Class를 저장할 변수

    // Start is called before the first frame update
    private void Start()
    {
        // FirePosition 하위에 있는 Component 추출
        muzzleFlash = firePosition.GetComponentInChildren<ParticleSystem>();

        // AudioSource Component 추출
        audio = GetComponent<AudioSource>();

        // shake Scrpit 추출
        shake = GameObject.Find("CameraRig").GetComponent<ShakeCamera>();
    }

    // Update is called once per frame
    private void Update()
    {
        // 마우스 왼쪽 버튼을 클릭했을 때, Fire 함수 호출
        if (!isReloading && Input.GetMouseButtonDown(0))
        {
            // 총알 수를 하나 감소
            --remainingBullet;
            Fire();

            // 남은 총알이 없을 경우 재장전 코루틴 호출
            if (remainingBullet == 0)
            {
                StartCoroutine(Reloading());
            }
        }
    }

    private void Fire()
    {
        // Shake Effect 호출
        StartCoroutine(shake.CameraShake());

        // Bullet Prefab을 동적으로 생성
        // Instantiate(bullet, firePosition.position, firePosition.rotation);

        var _bullet = GameManager.instance.GetBullet();
        if (_bullet != null)
        {
            _bullet.transform.position = firePosition.position;
            _bullet.transform.rotation = firePosition.rotation;
            _bullet.SetActive(true);
        }

        // 파티클 실행
        // cartridge.Play();

        // 총구 화염 파티클 실행
        muzzleFlash.Play();

        // 재장전 이미지의 fillAmount 속성값 지정
        magazineImage.fillAmount = (float)remainingBullet / (float)maxBullet;
        // 남은 총알 수 갱신
        UpdateBulletText();
    }

    private IEnumerator Reloading()
    {
        isReloading = true;
        audio.PlayOneShot(playerSFX.reload[(int)currentWeapon], 1.0f);

        // 재장전 오디오의 길이 + 0.3초 동안 대기
        yield return new WaitForSeconds(playerSFX.reload[(int)currentWeapon].length + 0.3f);

        // 각 종 변수값의 초기화
        isReloading = false;
        magazineImage.fillAmount = 1.0f;
        remainingBullet = maxBullet;
        // 남은 총알 수 갱신

        UpdateBulletText();
    }

    private void UpdateBulletText()
    {
        // 남은 총알 수 / 최대 총알 수 표시
        magazineText.text = string.Format("<color=#ff0000>{0}</color>/{1}", remainingBullet, maxBullet);
    }

    private void FireSFX()
    {
        // 현재 들고 있는 무기의 오디오 클립 호출
        var sfx = playerSFX.fire[(int)currentWeapon];

        // 사운드 출력
        audio.PlayOneShot(sfx, 1.0f);
    }
}
